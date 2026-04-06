using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[Controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get() =>await _context.Products.ToListAsync();
        //await _context.Products.ToListAsync();

    /*[HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }*/
    [HttpPost]
public async Task<IActionResult> CreateProduct(CreateProductDto dto)
{
    var product = new Product
    {
        Name = dto.Name,
        BasePrice = dto.BasePrice,
        ProductGroupId = dto.ProductGroupId,
        StockQuantity = dto.StockQuantity
    };

    _context.Products.Add(product);
    await _context.SaveChangesAsync();

    return Ok(product);
}

    [HttpPut("{id}")]
public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
{
    var product = await _context.Products.FindAsync(id);
    if (product == null)
        return NotFound();

    product.Name = dto.Name;
    product.BasePrice = dto.BasePrice;
    product.ProductGroupId = dto.ProductGroupId;

    // Készlet növelése vagy csökkentése
    product.StockQuantity += dto.AddStock;

    await _context.SaveChangesAsync();
    return Ok(product);
}

}