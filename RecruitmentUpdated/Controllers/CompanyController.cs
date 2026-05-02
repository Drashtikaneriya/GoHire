using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Company;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CompanyController(AppDbContext db) => _db = db;

        // GET /companies — Admin, HR Manager
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _db.Companies
                .Select(c => new CompanyResponseDTO
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    Industry = c.Industry,
                    Email = c.Email,
                    Phone = c.Phone,
                    Website = c.Website,
                    Address = c.Address,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();

            return Ok(companies);
        }

        // GET /companies/{id} — Admin, HR Manager
        [Authorize(Roles = "Admin,HR Manager")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _db.Companies
                .Where(c => c.CompanyId == id)
                .Select(c => new CompanyResponseDTO
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    Industry = c.Industry,
                    Email = c.Email,
                    Phone = c.Phone,
                    Website = c.Website,
                    Address = c.Address,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (company == null)
                return NotFound(new { message = "Company not found." });

            return Ok(company);
        }

        // POST /companies — Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDTO dto)
        {
            var company = new Company
            {
                CompanyName = dto.CompanyName,
                Industry = dto.Industry,
                Email = dto.Email,
                Phone = dto.Phone,
                Website = dto.Website,
                Address = dto.Address,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _db.Companies.Add(company);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = company.CompanyId },
                new { message = "Company created successfully.", companyId = company.CompanyId });
        }

        // PUT /companies/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUpdateDTO dto)
        {
            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return NotFound(new { message = "Company not found." });

            company.CompanyName = dto.CompanyName;
            company.Industry = dto.Industry;
            company.Email = dto.Email;
            company.Phone = dto.Phone;
            company.Website = dto.Website;
            company.Address = dto.Address;
            company.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Company updated successfully." });
        }

        // DELETE /companies/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return NotFound(new { message = "Company not found." });

            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Company deleted successfully." });
        }
    }
}