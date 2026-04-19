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
    public async Task<IEnumerable<ProductGroup>> Get()
        => await _context.ProductGroups
            .Include(g => g.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> CreateGroup(CreateProductGroupDto dto)
    {
        var group = new ProductGroup
        {
            Name = dto.Name,
            Items = dto.Items.Select(i => new ProductGroupItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _context.ProductGroups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(group);
    }
}
