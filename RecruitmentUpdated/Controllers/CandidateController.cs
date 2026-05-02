using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Candidate;
using RecruitmentsystemAPI.Models;
using RecruitmentsystemAPI.Services;
using System.Security.Claims;

namespace RecruitmentsystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/candidates")]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;

        public CandidateController(AppDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("UserId") ?? "0");

        private string GetCurrentUserRole() =>
            User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            var candidate = await _db.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
                return NotFound(new { message = "Candidate profile not found." });

            return Ok(MapToResponseDTO(candidate));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var candidate = await _db.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null)
                return NotFound(new { message = "Candidate not found." });

            var currentUserId = GetCurrentUserId();
            var currentRole = GetCurrentUserRole();

            // Candidates can only view their own profile
            if (currentRole == "Candidate" && candidate.UserId != currentUserId)
                return Forbid();

            // Interviwer has access to view candidate details
            if (currentRole == "Interviewer")
            {
                // Logic can be added here to restrict to assigned candidates only if needed
                // For now, allow viewing since they need to see resumes
            }

            return Ok(MapToResponseDTO(candidate));
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var candidates = await _db.Candidates
                .Include(c => c.User)
                .ToListAsync();

            var dtos = candidates.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CandidateCreateDTO dto)
        {
            var userId = GetCurrentUserId();

            // Only one profile per user allowed
            var exists = await _db.Candidates.AnyAsync(c => c.UserId == userId);
            if (exists)
                return Conflict(new { message = "Candidate profile already exists for this user." });

            string? resumePath = null;
            if (dto.DefaultResumeFile != null)
            {
                resumePath = await _fileService.UploadFileAsync(dto.DefaultResumeFile, "resumes");
            }

            var candidate = new Candidate
            {
                UserId = userId,
                LinkedInUrl = dto.LinkedInUrl,
                PortfolioUrl = dto.PortfolioUrl,
                DefaultResumePath = resumePath,
                Created = DateTime.Now
            };

            _db.Candidates.Add(candidate);
            await _db.SaveChangesAsync();

            // Refresh to get user details for Response DTO
            await _db.Entry(candidate).Reference(c => c.User).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = candidate.CandidateId }, MapToResponseDTO(candidate));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CandidateUpdateDTO dto)
        {
            var candidate = await _db.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null)
                return NotFound(new { message = "Candidate not found." });

            if (candidate.UserId != GetCurrentUserId())
                return Forbid();

            if (dto.DefaultResumeFile != null)
            {
                // Delete old resume if it exists
                if (!string.IsNullOrEmpty(candidate.DefaultResumePath))
                {
                    _fileService.DeleteFile(candidate.DefaultResumePath);
                }
                candidate.DefaultResumePath = await _fileService.UploadFileAsync(dto.DefaultResumeFile, "resumes");
            }

            candidate.LinkedInUrl = dto.LinkedInUrl;
            candidate.PortfolioUrl = dto.PortfolioUrl;

            await _db.SaveChangesAsync();

            return Ok(MapToResponseDTO(candidate));
        }

        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDTO dto)
        {
            var userId = GetCurrentUserId();
            var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
                return NotFound(new { message = "Candidate profile not found. Please create it first." });

            if (!string.IsNullOrEmpty(candidate.DefaultResumePath))
            {
                _fileService.DeleteFile(candidate.DefaultResumePath);
            }

            candidate.DefaultResumePath = await _fileService.UploadFileAsync(dto.Resume, "resumes");
            await _db.SaveChangesAsync();

            return Ok(new { message = "Resume uploaded successfully.", path = candidate.DefaultResumePath });
        }

        private CandidateResponseDTO MapToResponseDTO(Candidate c)
        {
            return new CandidateResponseDTO
            {
                CandidateId = c.CandidateId,
                UserId = c.UserId,
                FullName = c.User?.FullName ?? "Unknown",
                Email = c.User?.Email ?? "Unknown",
                Phone = c.User?.Phone,
                LinkedInUrl = c.LinkedInUrl,
                PortfolioUrl = c.PortfolioUrl,
                DefaultResumePath = c.DefaultResumePath,
                Created = c.Created
            };
        }
    }
}
