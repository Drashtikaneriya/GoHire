using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs.Company;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CompaniesController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL =================
        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _db.Company
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

        // ================= GET BY ID =================
        [Authorize(Roles = "Admin,HR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _db.Company
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
                return NotFound(new { message = "Company not found" });

            return Ok(company);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CompanyCreateDTO dto)
        {
            // ✅ FluentValidation auto runs

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

            _db.Company.Add(company);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Company created successfully",
                companyId = company.CompanyId
            });
        }

        // ================= UPDATE =================
        [Authorize(Roles = "Admin,HR")]
        [HttpPut]
        public async Task<IActionResult> Update(CompanyUpdateDTO dto)
        {
            var existing = await _db.Company
                .FirstOrDefaultAsync(c => c.CompanyId == dto.CompanyId);

            if (existing == null)
                return NotFound(new { message = "Company not found" });

            existing.CompanyName = dto.CompanyName;
            existing.Industry = dto.Industry;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Website = dto.Website;
            existing.Address = dto.Address;
            existing.IsActive = dto.IsActive;
            existing.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Company updated successfully" });
        }

        // ================= SOFT DELETE =================
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _db.Company.FindAsync(id);
            if (company == null)
                return NotFound(new { message = "Company not found" });

            company.IsActive = false;
            company.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Company deactivated successfully" });
        }
    }
}
