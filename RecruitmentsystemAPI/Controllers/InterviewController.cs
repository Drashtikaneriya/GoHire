using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs.Interview;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InterviewsController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var interviews = await _db.Interviews
                .Select(i => new InterviewResponseDTO
                {
                    InterviewId = i.InterviewId,
                    ApplicationId = i.ApplicationId,
                    InterviewerId = i.InterviewerId,
                    InterviewDate = i.InterviewDate,
                    Mode = i.Mode,
                    RoundNo = i.RoundNo,
                    Feedback = i.Feedback,
                    Result = i.Result,
                    CreatedDate = i.CreatedDate
                })
                .ToListAsync();

            return Ok(interviews);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interview = await _db.Interviews
                .Where(i => i.InterviewId == id)
                .Select(i => new InterviewResponseDTO
                {
                    InterviewId = i.InterviewId,
                    ApplicationId = i.ApplicationId,
                    InterviewerId = i.InterviewerId,
                    InterviewDate = i.InterviewDate,
                    Mode = i.Mode,
                    RoundNo = i.RoundNo,
                    Feedback = i.Feedback,
                    Result = i.Result,
                    CreatedDate = i.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (interview == null)
                return NotFound(new { message = "Interview not found" });

            return Ok(interview);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> Create(InterviewCreateDTO dto)
        {
            // ✅ FluentValidation auto runs

            var interview = new Interview
            {
                ApplicationId = dto.ApplicationId,
                InterviewerId = dto.InterviewerId,
                InterviewDate = dto.InterviewDate,
                Mode = dto.Mode,
                RoundNo = dto.RoundNo,
                Feedback = dto.Feedback,
                Result = "Pending",
                CreatedDate = DateTime.Now
            };

            _db.Interviews.Add(interview);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Interview scheduled successfully",
                interviewId = interview.InterviewId
            });
        }

        // ================= UPDATE =================
        [HttpPut]
        public async Task<IActionResult> Update(InterviewUpdateDTO dto)
        {
            var existing = await _db.Interviews
                .FirstOrDefaultAsync(i => i.InterviewId == dto.InterviewId);

            if (existing == null)
                return NotFound(new { message = "Interview not found" });

            existing.InterviewDate = dto.InterviewDate;
            existing.Mode = dto.Mode;
            existing.RoundNo = dto.RoundNo;
            existing.Feedback = dto.Feedback;
            existing.Result = dto.Result;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview updated successfully" });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null)
                return NotFound(new { message = "Interview not found" });

            _db.Interviews.Remove(interview);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview deleted successfully" });
        }
    }
}
