using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Jobposition;
using RecruitmentsystemAPI.Models;
using System.Security.Claims;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobPositionController : ControllerBase
    {
        private readonly AppDbContext _db;

        public JobPositionController(AppDbContext db) => _db = db;

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("UserId") ?? "0");

        private static JobPositionResponseDTO MapJob(JobPosition j) => new()
        {
            JobId = j.JobId,
            CompanyId = j.CompanyId,
            CompanyName = j.Company?.CompanyName ?? string.Empty,
            Title = j.Title,
            Description = j.Description,
            Location = j.Location,
            EmploymentType = j.EmploymentType,
            SalaryMin = j.SalaryMin,
            SalaryMax = j.SalaryMax,
            ClosingDate = j.ClosingDate,
            IsActive = j.IsActive,
            CreatedByUserId = j.CreatedByUserId,
            CreatedByFullName = j.CreatedByUser?.FullName ?? string.Empty,
            CreatedDate = j.CreatedDate,
            ModifiedDate = j.ModifiedDate
        };

        // GET /jobs — Admin, HR Manager, Candidate
        [Authorize(Roles = "Admin,HR Manager,Candidate")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _db.JobPositions
                .Include(j => j.Company)
                .Include(j => j.CreatedByUser)
                .ToListAsync();

            return Ok(jobs.Select(MapJob));
        }

        // GET /jobs/{id} — Admin, HR Manager, Candidate
        [Authorize(Roles = "Admin,HR Manager,Candidate")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _db.JobPositions
                .Include(j => j.Company)
                .Include(j => j.CreatedByUser)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                return NotFound(new { message = "Job position not found." });

            return Ok(MapJob(job));
        }

        // GET /jobs/active — Admin, HR Manager, Candidate (returns only active jobs)
        [Authorize(Roles = "Admin,HR Manager,Candidate")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var jobs = await _db.JobPositions
                .Include(j => j.Company)
                .Include(j => j.CreatedByUser)
                .Where(j => j.IsActive)
                .ToListAsync();

            return Ok(jobs.Select(MapJob));
        }

        // GET /jobs/search?title=&location=&employmentType=&companyId= — Candidate
        [Authorize(Roles = "Candidate")]
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? title,
            [FromQuery] string? location,
            [FromQuery] string? employmentType,
            [FromQuery] int? companyId)
        {
            var query = _db.JobPositions
                .Include(j => j.Company)
                .Include(j => j.CreatedByUser)
                .Where(j => j.IsActive) // Candidates only see active jobs
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(j => j.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(j => j.Location != null && j.Location.Contains(location));

            if (!string.IsNullOrWhiteSpace(employmentType))
                query = query.Where(j => j.EmploymentType != null && j.EmploymentType == employmentType);

            if (companyId.HasValue)
                query = query.Where(j => j.CompanyId == companyId.Value);

            var jobs = await query.ToListAsync();
            return Ok(jobs.Select(MapJob));
        }

        // GET /jobs/company/{companyId} — Admin, HR Manager
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var companyExists = await _db.Companies.AnyAsync(c => c.CompanyId == companyId);
            if (!companyExists)
                return NotFound(new { message = "Company not found." });

            var jobs = await _db.JobPositions
                .Include(j => j.Company)
                .Include(j => j.CreatedByUser)
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();

            return Ok(jobs.Select(MapJob));
        }

        // POST /jobs — HR Manager, Admin
        [Authorize(Roles = "HR Manager,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobPositionCreateDTO dto)
        {
            var companyExists = await _db.Companies.AnyAsync(c => c.CompanyId == dto.CompanyId);
            if (!companyExists)
                return BadRequest(new { message = "Invalid CompanyId." });

            // Auto-assign createdBy to the current user
            var createdByUserId = GetCurrentUserId();

            var job = new JobPosition
            {
                CompanyId = dto.CompanyId,
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                EmploymentType = dto.EmploymentType,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                ClosingDate = dto.ClosingDate,
                IsActive = true,
                CreatedByUserId = createdByUserId,
                CreatedDate = DateTime.Now
            };

            _db.JobPositions.Add(job);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = job.JobId },
                new { message = "Job position created successfully.", jobId = job.JobId });
        }

        // PUT /jobs/{id} — HR Manager (own), Admin
        [Authorize(Roles = "HR Manager,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobPositionUpdateDTO dto)
        {
            var job = await _db.JobPositions.FindAsync(id);
            if (job == null)
                return NotFound(new { message = "Job position not found." });

            // HR Managers can only edit their own postings
            if (User.IsInRole("HR Manager") && job.CreatedByUserId != GetCurrentUserId())
                return Forbid();

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Location = dto.Location;
            job.EmploymentType = dto.EmploymentType;
            job.SalaryMin = dto.SalaryMin;
            job.SalaryMax = dto.SalaryMax;
            job.ClosingDate = dto.ClosingDate;
            job.IsActive = dto.IsActive;
            job.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Job position updated successfully." });
        }

        // DELETE /jobs/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _db.JobPositions.FindAsync(id);
            if (job == null)
                return NotFound(new { message = "Job position not found." });

            _db.JobPositions.Remove(job);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Job position deleted successfully." });
        }

        // PATCH /jobs/{id}/status — HR Manager (own), Admin
        [Authorize(Roles = "HR Manager,Admin")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] JobStatusDTO dto)
        {
            var job = await _db.JobPositions.FindAsync(id);
            if (job == null)
                return NotFound(new { message = "Job position not found." });

            // HR Managers can only toggle their own postings
            if (User.IsInRole("HR Manager") && job.CreatedByUserId != GetCurrentUserId())
                return Forbid();

            job.IsActive = dto.IsActive;
            job.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = $"Job status set to {(dto.IsActive ? "active" : "inactive")}." });
        }
    }
}