using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class QueryController : ControllerBase
{
    private readonly AppDbContext _context;

    public QueryController(AppDbContext context)
    {
        _context = context;
    }

    // 1) Legtöbbet értékesített termék
    [HttpGet("top-product")]
    public async Task<IActionResult> GetTopProduct()
    {
        var result = await _context.SaleLines
            .Include(sl => sl.Product)
            .GroupBy(sl => sl.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                ProductName = g.First().Product.Name,
                TotalQuantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefaultAsync();

        return Ok(result);
    }

    // 2) Legtöbbet vásárló partner
    [HttpGet("top-partner")]
    public async Task<IActionResult> GetTopPartner()
    {
        var result = await _context.Sales
            .Include(s => s.Partner)
            .GroupBy(s => s.PartnerId)
            .Select(g => new
            {
                PartnerId = g.Key,
                PartnerName = g.First().Partner.Name,
                TotalSpent = g.Sum(x => x.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .FirstOrDefaultAsync();

        return Ok(result);
    }
}
