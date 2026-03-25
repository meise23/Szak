using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PackagesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PackagesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Package>> Get() =>
        await _context.Packages
            .Include(p => p.Items)
            //.Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(Package package)
    {
        _context.Packages.Add(package);
        await _context.SaveChangesAsync();
        return Ok(package);
    }
}
