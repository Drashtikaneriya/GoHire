using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Interview;
using RecruitmentsystemAPI.Models;
using System.Security.Claims;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InterviewsController(AppDbContext db) => _db = db;

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("UserId") ?? "0");

        private static InterviewResponseDTO MapInterview(Interview i) => new()
        {
            InterviewId = i.InterviewId,
            ApplicationId = i.ApplicationId,
            CandidateName = i.Application?.Candidate?.User?.FullName ?? string.Empty,
            JobTitle = i.Application?.JobPosition?.Title ?? string.Empty,
            RoundId = i.RoundId,
            RoundName = i.Round?.RoundName ?? string.Empty,
            InterviewerUserId = i.InterviewerUserId,
            InterviewerName = i.Interviewer?.FullName ?? string.Empty,
            InterviewDate = i.InterviewDate,
            InterviewEnd = i.InterviewEnd,
            Mode = i.Mode,
            Feedback = i.Feedback,
            Result = i.Result,
            UpdatedByUserId = i.UpdatedByUserId,
            CreatedDate = i.CreatedDate,
            ModifiedDate = i.ModifiedDate
        };

        private IQueryable<Interview> BaseQuery() =>
            _db.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a!.Candidate)
                        .ThenInclude(c => c!.User)
                .Include(i => i.Application)
                    .ThenInclude(a => a!.JobPosition)
                .Include(i => i.Round)
                .Include(i => i.Interviewer);

        // POST /interviews — Admin, HR Manager schedules an interview
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InterviewCreateDTO dto)
        {
            var appExists = await _db.Applications.AnyAsync(a => a.ApplicationId == dto.ApplicationId);
            if (!appExists)
                return BadRequest(new { message = "Invalid ApplicationId." });

            var roundExists = await _db.InterviewRounds.AnyAsync(r => r.RoundId == dto.RoundId);
            if (!roundExists)
                return BadRequest(new { message = "Invalid RoundId." });

            var interviewerExists = await _db.Users.AnyAsync(u => u.UserId == dto.InterviewerUserId);
            if (!interviewerExists)
                return BadRequest(new { message = "Invalid InterviewerUserId." });

            var interview = new Interview
            {
                ApplicationId = dto.ApplicationId,
                RoundId = dto.RoundId,
                InterviewerUserId = dto.InterviewerUserId,
                InterviewDate = dto.InterviewDate,
                InterviewEnd = dto.InterviewEnd,
                Mode = dto.Mode,
                Result = "Pending",
                CreatedDate = DateTime.Now
            };

            _db.Interviews.Add(interview);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = interview.InterviewId },
                new { message = "Interview scheduled successfully.", interviewId = interview.InterviewId });
        }

        // GET /interviews — Admin, HR Manager (all interviews)
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var interviews = await BaseQuery()
                .OrderByDescending(i => i.InterviewDate)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // DELETE /interviews/{id} — Admin, HR Manager
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            _db.Interviews.Remove(interview);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview deleted successfully." });
        }

        // GET /interviews/{id} — Admin, HR Manager, Interviewer (if assigned)
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interview = await BaseQuery()
                .FirstOrDefaultAsync(i => i.InterviewId == id);

            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            // Interviewers can only view interviews assigned to them
            if (User.IsInRole("Interviewer") && interview.InterviewerUserId != GetCurrentUserId())
                return Forbid();

            return Ok(MapInterview(interview));
        }

        // PUT /interviews/{id} — Admin, HR Manager updates scheduling details
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InterviewUpdateDTO dto)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            if (dto.InterviewDate.HasValue) interview.InterviewDate = dto.InterviewDate.Value;
            if (dto.InterviewEnd.HasValue) interview.InterviewEnd = dto.InterviewEnd.Value;
            if (dto.Mode != null) interview.Mode = dto.Mode;
            if (dto.Result != null) interview.Result = dto.Result;

            interview.UpdatedByUserId = GetCurrentUserId();
            interview.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview updated successfully." });
        }

        // GET /interviews/application/{applicationId} — Admin, HR Manager, Interviewer
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplication(int applicationId)
        {
            var appExists = await _db.Applications.AnyAsync(a => a.ApplicationId == applicationId);
            if (!appExists)
                return NotFound(new { message = "Application not found." });

            var interviews = await BaseQuery()
                .Where(i => i.ApplicationId == applicationId)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // GET /interviews/interviewer/{userId} — Admin (all), Interviewer (own only)
        [Authorize(Roles = "Admin,Interviewer")]
        [HttpGet("interviewer/{userId}")]
        public async Task<IActionResult> GetByInterviewer(int userId)
        {
            // Interviewers can only query their own schedule; Admin can query any
            if (User.IsInRole("Interviewer") && userId != GetCurrentUserId())
                return Forbid();

            var interviews = await BaseQuery()
                .Where(i => i.InterviewerUserId == userId)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // GET /interviews/upcoming — Admin/HR Manager (all), Interviewer (own upcoming)
        [Authorize(Roles = "Admin,Interviewer,HR Manager")]
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var now = DateTime.Now;
            IQueryable<Interview> query = BaseQuery()
                .Where(i => i.InterviewDate >= now);

            // Interviewers see only their own upcoming interviews
            if (User.IsInRole("Interviewer"))
                query = query.Where(i => i.InterviewerUserId == GetCurrentUserId());

            var interviews = await query
                .OrderBy(i => i.InterviewDate)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // GET /interviews/my — Returns interviews for the current user (candidate or interviewer)
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyInterviews()
        {
            var userId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);

            IQueryable<Interview> query = BaseQuery();

            if (role == "Interviewer")
            {
                query = query.Where(i => i.InterviewerUserId == userId);
            }
            else if (role == "Candidate")
            {
                query = query.Where(i => i.Application!.Candidate!.UserId == userId);
            }
            else if (role != "Admin" && role != "HR Manager")
            {
                return Forbid();
            }

            var interviews = await query
                .OrderByDescending(i => i.InterviewDate)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // GET /interviews/candidate/{candidateId} — Admin/HR Manager or the candidate themselves
        [Authorize]
        [HttpGet("candidate/{candidateId:int}")]
        public async Task<IActionResult> GetByCandidate(int candidateId)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = GetCurrentUserId();

            if (role == "Candidate")
            {
                var candidate = await _db.Candidates.FirstOrDefaultAsync(c => c.UserId == userId);
                if (candidate == null || candidate.CandidateId != candidateId)
                    return Forbid();
            }
            else if (role != "Admin" && role != "HR Manager")
            {
                return Forbid();
            }

            var interviews = await BaseQuery()
                .Where(i => i.Application!.CandidateId == candidateId)
                .OrderByDescending(i => i.InterviewDate)
                .ToListAsync();

            return Ok(interviews.Select(MapInterview));
        }

        // PATCH /interviews/{id}/feedback — Interviewer submits feedback
        [Authorize(Roles = "Interviewer")]
        [HttpPatch("{id:int}/feedback")]
        public async Task<IActionResult> SubmitFeedback(int id, [FromBody] InterviewFeedbackDTO dto)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            // Only the assigned interviewer can submit feedback
            if (interview.InterviewerUserId != GetCurrentUserId())
                return Forbid();

            interview.Feedback = dto.Feedback;
            interview.UpdatedByUserId = GetCurrentUserId();
            interview.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Feedback submitted successfully." });
        }

        // PATCH /interviews/{id}/result — Admin, HR Manager sets final result
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPatch("{id:int}/result")]
        public async Task<IActionResult> SetResult(int id, [FromBody] InterviewResultDTO dto)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            interview.Result = dto.Result;
            interview.UpdatedByUserId = GetCurrentUserId();
            interview.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = $"Interview result set to '{dto.Result}'." });
        }
    }
}
