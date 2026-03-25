using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ImportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("products")]
    public async Task<IActionResult> ImportProducts(List<ProductImportItemDto> items)
    {
        var results = new List<object>();

        foreach (var item in items)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Sku == item.Sku);

            if (product == null)
            {
                // Új termék létrehozása
                product = new Product
                {
                    Sku = item.Sku,
                    Name = item.Name,
                    BasePrice = item.BasePrice,
                    StockQuantity = item.Quantity
                };

                _context.Products.Add(product);

                results.Add(new
                {
                    Sku = item.Sku,
                    Status = "Created",
                    QuantityAdded = item.Quantity
                });
            }
            else
            {
                // Meglévő termék készletének növelése
                product.StockQuantity += item.Quantity;

                // Ha az ár változik, frissítjük
                if (product.BasePrice != item.BasePrice)
                    product.BasePrice = item.BasePrice;

                results.Add(new
                {
                    Sku = item.Sku,
                    Status = "Updated",
                    QuantityAdded = item.Quantity
                });
            }
        }

        await _context.SaveChangesAsync();

        return Ok(results);
    }
}
