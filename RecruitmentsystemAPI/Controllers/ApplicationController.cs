using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO;
using RecruitmentsystemAPI.DTOs.Application;
using RecruitmentsystemAPI.Models;
using static Application;

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

        //// GET: api/applications
        //[Authorize(Roles = "Admin,HR")]
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var applications = await _db.Applications
        //            .Include(a => a.JobPosition)
        //            .Include(a => a.Candidate)
        //            .ToListAsync();

        //        return Ok(applications);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Error getting applications", error = ex.Message });
        //    }
        //}

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var applications = await _db.Applications
                    .Select(a => new ApplicationResponseDTO
                    {
                        ApplicationId = a.ApplicationId,
                        JobId = a.JobId,
                        JobTitle = _db.JobPositions
                            .Where(j => j.JobId == a.JobId)
                            .Select(j => j.Title)
                            .FirstOrDefault(),

                        CandidateId = a.CandidateId,
                        CandidateName = _db.Candidates
                            .Where(c => c.CandidateId == a.CandidateId)
                            .Select(c => c.FullName)
                            .FirstOrDefault(),

                        Status = a.Status ?? "Applied",
                        HRNotes = a.HRNotes,
                        AppliedOn = a.AppliedOn,
                        ModifiedDate = a.ModifiedDate
                    })
                    .ToListAsync();

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error getting applications",
                    error = ex.Message
                });
            }
        }


        // GET: api/applications/5
        [Authorize(Roles = "Admin,HR,Candidate")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var application = await _db.Applications
                    .Where(a => a.ApplicationId == id)
                    .Select(a => new ApplicationResponseDTO
                    {
                        ApplicationId = a.ApplicationId,
                        JobId         = a.JobId,
                        JobTitle      = _db.JobPositions
                                            .Where(j => j.JobId == a.JobId)
                                            .Select(j => j.Title)
                                            .FirstOrDefault(),
                        CandidateId   = a.CandidateId,
                        CandidateName = _db.Candidates
                                            .Where(c => c.CandidateId == a.CandidateId)
                                            .Select(c => c.FullName)
                                            .FirstOrDefault(),
                        Status        = a.Status ?? "Applied",
                        HRNotes       = a.HRNotes,
                        AppliedOn     = a.AppliedOn,
                        ModifiedDate  = a.ModifiedDate
                    })
                    .FirstOrDefaultAsync();

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
        [Authorize(Roles = "Candidate")]
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


        // PUT: api/applications/5  ← HR updates Status and HRNotes only
        [Authorize(Roles = "Admin,HR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ApplicationUpdateDTO dto)
        {
            try
            {
                // Route id must match body ApplicationId (if provided)
                if (dto.ApplicationId != 0 && dto.ApplicationId != id)
                    return BadRequest(new { message = "Application ID mismatch between route and body." });

                var existing = await _db.Applications.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Application not found" });

                // Only update the HR-controlled fields
                if (!string.IsNullOrWhiteSpace(dto.Status))
                    existing.Status = dto.Status;

                existing.HRNotes     = dto.HRNotes;  // allow clearing notes
                existing.ModifiedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message        = "Application updated successfully",
                    applicationId  = existing.ApplicationId,
                    status         = existing.Status,
                    modifiedDate   = existing.ModifiedDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating application", error = ex.Message });
            }
        }

        // DELETE: api/applications/5
        [Authorize(Roles = "Admin")]
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
