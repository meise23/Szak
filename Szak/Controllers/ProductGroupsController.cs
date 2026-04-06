using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductGroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductGroupsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductGroup>> Get() =>await _context.ProductGroups.Include(g => g.Products).ToListAsync();
        //await _context.ProductGroups.Include(g => g.Products).ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(ProductGroup group)
    {
        _context.ProductGroups.Add(group);
        await _context.SaveChangesAsync();
        return Ok(group);
    }
    [HttpPost]
public async Task<IActionResult> CreateGroup(CreateProductGroupDto dto)
{
    var group = new ProductGroup
    {
        Name = dto.Name
    };

    _context.ProductGroups.Add(group);
    await _context.SaveChangesAsync();

    return Ok(group);
}

}
