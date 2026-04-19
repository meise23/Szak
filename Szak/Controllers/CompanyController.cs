using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompanyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        
        var company = await _context.Companies.FirstOrDefaultAsync();
        if (company == null)
    {
        return Ok(new {
            name = "",
            director = "",
            address = ""
        });
    }
        return Ok(company);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CompanyDto dto)
    {
        var company = await _context.Companies.FirstOrDefaultAsync();

        if (company == null)
        {
            company = new Company();
            _context.Companies.Add(company);
        }

        company.Name = dto.Name;
        company.Director = dto.Director;
        company.Address = dto.Address;

        await _context.SaveChangesAsync();

        return Ok(company);
    }
}
