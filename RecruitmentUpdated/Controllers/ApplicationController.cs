using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Application;
using RecruitmentsystemAPI.Models;
using RecruitmentsystemAPI.Services;
using System.Security.Claims;

namespace RecruitmentsystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;

        public ApplicationController(AppDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("UserId") ?? "0");

        private string GetCurrentUserRole() =>
            User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        [HttpGet("my")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = GetCurrentUserId();
            var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
                return NotFound(new { message = "Candidate profile not found." });

            var applications = await _db.Applications
                .Include(a => a.JobPosition)
                .Include(a => a.Candidate)
                .ThenInclude(c => c!.User)
                .Where(a => a.CandidateId == candidate.CandidateId)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            var dtos = applications.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _db.Applications
                .Include(a => a.JobPosition)
                .Include(a => a.Candidate)
                .ThenInclude(c => c!.User)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                return NotFound(new { message = "Application not found." });

            var currentUserId = GetCurrentUserId();
            var currentRole = GetCurrentUserRole();

            // Candidate can only view their own
            if (currentRole == "Candidate")
            {
                var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == currentUserId);
                if (candidate == null || application.CandidateId != candidate.CandidateId)
                    return Forbid();
            }
            else if (currentRole != "Admin" && currentRole != "HR Manager")
            {
                return Forbid();
            }

            return Ok(MapToResponseDTO(application));
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _db.Applications
                .Include(a => a.JobPosition)
                .Include(a => a.Candidate)
                .ThenInclude(c => c!.User)
                .ToListAsync();

            var dtos = applications.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(int jobId)
        {
            var applications = await _db.Applications
                .Include(a => a.JobPosition)
                .Include(a => a.Candidate)
                .ThenInclude(c => c!.User)
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            var dtos = applications.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ApplicationCreateDTO dto)
        {
            var userId = GetCurrentUserId();
            var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
                return BadRequest(new { message = "You must create a candidate profile before applying for jobs." });

            // Check duplicate application
            var exists = await _db.Applications.AnyAsync(a => a.JobId == dto.JobId && a.CandidateId == candidate.CandidateId);
            if (exists)
                return Conflict(new { message = "You have already applied for this position." });

            string? resumePath = null;
            if (dto.ResumeFile != null)
            {
                resumePath = await _fileService.UploadFileAsync(dto.ResumeFile, "resumes");
            }
            else
            {
                resumePath = candidate.DefaultResumePath;
            }

            var application = new Application
            {
                JobId = dto.JobId,
                CandidateId = candidate.CandidateId,
                ResumePath = resumePath,
                Status = "Applied",
                AppliedDate = DateTime.Now
            };

            _db.Applications.Add(application);
            await _db.SaveChangesAsync();

            // Load related data for response
            await _db.Entry(application).Reference(a => a.JobPosition).LoadAsync();
            await _db.Entry(application).Reference(a => a.Candidate).LoadAsync();
            if (application.Candidate != null)
            {
                await _db.Entry(application.Candidate).Reference(c => c.User).LoadAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = application.ApplicationId }, MapToResponseDTO(application));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _db.Applications.FindAsync(id);
            if (application == null)
                return NotFound(new { message = "Application not found." });

            var currentUserId = GetCurrentUserId();
            var currentRole = GetCurrentUserRole();

            if (currentRole == "Candidate")
            {
                var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == currentUserId);
                if (candidate == null || application.CandidateId != candidate.CandidateId)
                    return Forbid();
            }
            else if (currentRole != "Admin" && currentRole != "HR Manager")
            {
                return Forbid();
            }

            _db.Applications.Remove(application);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Application deleted successfully." });
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDTO dto)
        {
            var application = await _db.Applications.FindAsync(id);
            if (application == null)
                return NotFound(new { message = "Application not found." });

            application.Status = dto.Status;
            application.HRNotes = dto.HRNotes;
            application.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Status updated successfully.", status = application.Status });
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPatch("{id}/shortlist")]
        public async Task<IActionResult> Shortlist(int id)
        {
            var application = await _db.Applications.FindAsync(id);
            if (application == null)
                return NotFound(new { message = "Application not found." });

            application.Status = "Shortlisted";
            application.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Candidate shortlisted.", status = application.Status });
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await _db.Applications.FindAsync(id);
            if (application == null)
                return NotFound(new { message = "Application not found." });

            application.Status = "Rejected";
            application.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Application rejected.", status = application.Status });
        }

        private ApplicationResponseDTO MapToResponseDTO(Application a)
        {
            return new ApplicationResponseDTO
            {
                ApplicationId = a.ApplicationId,
                JobId = a.JobId,
                JobTitle = a.JobPosition?.Title ?? "Unknown",
                CandidateId = a.CandidateId,
                CandidateFullName = a.Candidate?.User?.FullName ?? "Unknown",
                ResumePath = a.ResumePath,
                Status = a.Status,
                HRNotes = a.HRNotes,
                AppliedDate = a.AppliedDate,
                ModifiedDate = a.ModifiedDate
            };
        }

        public class UpdateStatusDTO
        {
            public string Status { get; set; } = string.Empty;
            public string? HRNotes { get; set; }
        }
    }
}
