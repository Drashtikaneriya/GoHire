using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs.Candidate;
using RecruitmentsystemAPI.Models;
using RecruitmentsystemAPI.Services;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;

        public CandidatesController(AppDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        // ================= GET ALL =================
        [Authorize(Roles = "Admin,HR")]
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
        [Authorize(Roles = "Admin,HR,Candidate")]
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
        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CandidateCreateDTO dto)
        {
            string? resumePath = null;

            if (dto.ResumePath == null)
            {
                resumePath = await _fileService.UploadFileAsync(
                    dto.DocumentFile,
                    "Candidates"
                );
            }
            Console.WriteLine("Resume Path: " + resumePath);

            var candidate = new Candidate
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                ResumePath = resumePath,
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
        [Authorize(Roles = "Candidate")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CandidateUpdateDTO dto)
        {
            var existing = await _db.Candidates
                .FirstOrDefaultAsync(c => c.CandidateId == dto.CandidateId);

            if (existing == null)
                return NotFound(new { message = "Candidate not found" });

            // Replace resume if new file provided
            if (dto.ResumePath != null)
            {
                _fileService.DeleteFile(existing.ResumePath);

                existing.ResumePath = await _fileService.UploadFileAsync(
                    dto.documentFile,
                    "Candidates"
                );
            }

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Candidate updated successfully" });
        }

        // ================= DELETE =================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var candidate = await _db.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound(new { message = "Candidate not found" });

            _fileService.DeleteFile(candidate.ResumePath);

            _db.Candidates.Remove(candidate);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Candidate deleted successfully" });
        }
    }
}
