using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.InterviewRound;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/interview-rounds")]
    public class InterviewRoundsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InterviewRoundsController(AppDbContext db) => _db = db;

        // GET /interview-rounds — Admin, HR Manager, Interviewer
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rounds = await _db.InterviewRounds
                .Select(r => new InterviewRoundResponseDTO
                {
                    RoundId = r.RoundId,
                    RoundName = r.RoundName
                })
                .ToListAsync();

            return Ok(rounds);
        }

        // POST /interview-rounds — Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InterviewRoundCreateDTO dto)
        {
            var exists = await _db.InterviewRounds.AnyAsync(r => r.RoundName == dto.RoundName);
            if (exists)
                return Conflict(new { message = "An interview round with this name already exists." });

            var round = new InterviewRound { RoundName = dto.RoundName };

            _db.InterviewRounds.Add(round);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = round.RoundId },
                new { message = "Interview round created successfully.", roundId = round.RoundId });
        }

        // GET /interview-rounds/{id} — Admin, HR Manager, Interviewer
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var round = await _db.InterviewRounds
                .Where(r => r.RoundId == id)
                .Select(r => new InterviewRoundResponseDTO
                {
                    RoundId = r.RoundId,
                    RoundName = r.RoundName
                })
                .FirstOrDefaultAsync();

            if (round == null)
                return NotFound(new { message = "Interview round not found." });

            return Ok(round);
        }

        // PUT /interview-rounds/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InterviewRoundCreateDTO dto)
        {
            var round = await _db.InterviewRounds.FindAsync(id);
            if (round == null)
                return NotFound(new { message = "Interview round not found." });

            var nameTaken = await _db.InterviewRounds
                .AnyAsync(r => r.RoundName == dto.RoundName && r.RoundId != id);
            if (nameTaken)
                return Conflict(new { message = "Another interview round with this name already exists." });

            round.RoundName = dto.RoundName;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview round updated successfully." });
        }

        // DELETE /interview-rounds/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var round = await _db.InterviewRounds.FindAsync(id);
            if (round == null)
                return NotFound(new { message = "Interview round not found." });

            var hasInterviews = await _db.Interviews.AnyAsync(i => i.RoundId == id);
            if (hasInterviews)
                return Conflict(new { message = "Cannot delete a round that has associated interviews." });

            _db.InterviewRounds.Remove(round);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Interview round deleted successfully." });
        }
    }
}
