using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,HR,Candidate")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _db.JobPositions
                .Include(j => j.Company)
                .Select(j => new JobPositionResponseDTO
                {
                    JobId = j.JobId,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Type = j.Type,
                    SalaryRange = j.SalaryRange,
                    CompanyId = j.CompanyId,
                    CompanyName = j.Company != null ? j.Company.CompanyName : "Not Assigned",
                    CreatedBy = j.CreatedBy,
                    CreatedDate = j.CreatedDate
                })
                .ToListAsync();

            return Ok(jobs);
        }

        // ================= GET BY ID =================
        [Authorize(Roles = "Admin,HR,Candidate")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _db.JobPositions
                .Include(j => j.Company)
                .Where(j => j.JobId == id)
                .Select(j => new JobPositionResponseDTO
                {
                    JobId = j.JobId,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Type = j.Type,
                    SalaryRange = j.SalaryRange,
                    CompanyId = j.CompanyId,
                    CompanyName = j.Company != null ? j.Company.CompanyName : "Not Assigned",
                    CreatedBy = j.CreatedBy,
                    CreatedDate = j.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (job == null)
                return NotFound(new { message = "Job not found" });

            return Ok(job);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin,HR")]
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
                CompanyId = dto.CompanyId,
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
        [Authorize(Roles = "Admin,HR")]
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
            existing.CompanyId = dto.CompanyId;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Job updated successfully" });
        }

        // ================= DELETE =================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var job = await _db.JobPositions.FindAsync(id);
                if (job == null)
                    return NotFound(new { success = false, message = "Job position not found" });

                // Check for related applications (Foreign Key Constraint)
                var hasApplications = await _db.Applications.AnyAsync(a => a.JobId == id);
                if (hasApplications)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Cannot delete this job position because it has active job applications. Deactivate it instead." 
                    });
                }

                _db.JobPositions.Remove(job);
                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "Job position deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting: " + ex.Message });
            }
        }
    }
}
