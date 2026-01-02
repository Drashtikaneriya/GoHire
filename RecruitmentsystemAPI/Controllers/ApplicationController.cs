using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.Models;
using static Application;
using RecruitmentsystemAPI.DTO;
using RecruitmentsystemAPI.DTOs.Application;

namespace RecruitmentsystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ApplicationsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/applications
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var applications = await _db.Applications
                    .Include(a => a.JobPosition)
                    .Include(a => a.Candidate)
                    .ToListAsync();

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting applications", error = ex.Message });
            }
        }

        // GET: api/applications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var application = await _db.Applications
                    .Include(a => a.JobPosition)
                    .Include(a => a.Candidate)
                    .FirstOrDefaultAsync(a => a.ApplicationId == id);

                if (application == null)
                    return NotFound(new { message = "Application not found" });

                return Ok(application);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting application", error = ex.Message });
            }
        }

        // POST: api/applications
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationCreateDTO dto)
        {
            try
            {
                var application = new Application
                {
                    JobId = dto.JobId,
                    CandidateId = dto.CandidateId,
                    Status = string.IsNullOrEmpty(dto.Status) ? "Applied" : dto.Status,
                    HRNotes = dto.HRNotes,
                    AppliedOn = DateTime.Now
                };

                _db.Applications.Add(application);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Application submitted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating application", error = ex.Message });
            }
        }


        // PUT: api/applications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Application application)
        {
            try
            {
                var existing = await _db.Applications.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Application not found" });

                existing.JobId = application.JobId;
                existing.CandidateId = application.CandidateId;
                existing.Status = application.Status;
                existing.HRNotes = application.HRNotes;
                existing.ModifiedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating application", error = ex.Message });
            }
        }

        // DELETE: api/applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var application = await _db.Applications.FindAsync(id);
                if (application == null)
                    return NotFound(new { message = "Application not found" });

                _db.Applications.Remove(application);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Application deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting application", error = ex.Message });
            }
        }
    }
}
