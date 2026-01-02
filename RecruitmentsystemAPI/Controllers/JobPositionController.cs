using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs.JobPosition;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPositionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public JobPositionsController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _db.JobPositions
                .Select(j => new JobPositionResponseDTO
                {
                    JobId = j.JobId,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Type = j.Type,
                    SalaryRange = j.SalaryRange,
                    CreatedBy = j.CreatedBy,
                    CreatedDate = j.CreatedDate
                })
                .ToListAsync();

            return Ok(jobs);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _db.JobPositions
                .Where(j => j.JobId == id)
                .Select(j => new JobPositionResponseDTO
                {
                    JobId = j.JobId,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Type = j.Type,
                    SalaryRange = j.SalaryRange,
                    CreatedBy = j.CreatedBy,
                    CreatedDate = j.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (job == null)
                return NotFound(new { message = "Job not found" });

            return Ok(job);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> Create(JobPositionCreateDTO dto)
        {
            var job = new JobPosition
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Type = dto.Type,
                SalaryRange = dto.SalaryRange,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _db.JobPositions.Add(job);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Job position created successfully",
                jobId = job.JobId
            });
        }

        // ================= UPDATE =================
        [HttpPut]
        public async Task<IActionResult> Update(JobPositionUpdateDTO dto)
        {
            var existing = await _db.JobPositions
                .FirstOrDefaultAsync(j => j.JobId == dto.JobId);

            if (existing == null)
                return NotFound(new { message = "Job not found" });

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Location = dto.Location;
            existing.Type = dto.Type;
            existing.SalaryRange = dto.SalaryRange;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Job updated successfully" });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _db.JobPositions.FindAsync(id);
            if (job == null)
                return NotFound(new { message = "Job not found" });

            _db.JobPositions.Remove(job);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Job deleted successfully" });
        }
    }
}
