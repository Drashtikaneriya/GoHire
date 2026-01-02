using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs.Candidate;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CandidatesController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var candidates = await _db.Candidates
                .Select(c => new CandidateResponseDTO
                {
                    CandidateId = c.CandidateId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    ResumePath = c.ResumePath,
                    AppliedDate = c.AppliedDate
                })
                .ToListAsync();

            return Ok(candidates);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var candidate = await _db.Candidates
                .Where(c => c.CandidateId == id)
                .Select(c => new CandidateResponseDTO
                {
                    CandidateId = c.CandidateId,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    ResumePath = c.ResumePath,
                    AppliedDate = c.AppliedDate
                })
                .FirstOrDefaultAsync();

            if (candidate == null)
                return NotFound(new { message = "Candidate not found" });

            return Ok(candidate);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> Create(CandidateCreateDTO dto)
        {
            // ✅ FluentValidation auto runs here

            var candidate = new Candidate
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                ResumePath = dto.ResumePath,
                AppliedDate = DateTime.Now
            };

            _db.Candidates.Add(candidate);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Candidate created successfully",
                candidateId = candidate.CandidateId
            });
        }

        // ================= UPDATE =================
        [HttpPut]
        public async Task<IActionResult> Update(CandidateUpdateDTO dto)
        {
            var existing = await _db.Candidates
                .FirstOrDefaultAsync(c => c.CandidateId == dto.CandidateId);

            if (existing == null)
                return NotFound(new { message = "Candidate not found" });

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.ResumePath = dto.ResumePath;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Candidate updated successfully" });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var candidate = await _db.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound(new { message = "Candidate not found" });

            _db.Candidates.Remove(candidate);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Candidate deleted successfully" });
        }
    }
}
