
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesController(AppDbContext context)
    {
        _context = context;
    }

[HttpPost]
public async Task<IActionResult> CreateSale(CreateSaleDto dto)
{
    var partner = await _context.Partners.FindAsync(dto.PartnerId);
    if (partner == null)
        return BadRequest("Partner not found");

    var sale = new Sale
    {
        PartnerId = dto.PartnerId,
        Date = DateTime.UtcNow,
        Lines = new List<SaleLine>()
    };

    decimal total = 0;
    var now = DateTime.UtcNow;

    // Érvényes akciók lekérése
    var promotions = await _context.Promotions
        .Include(p => p.PromotionProducts)
        .Include(p => p.PromotionGroups)
        .ToListAsync();

    var activePromotions = promotions
        .Where(p => p.ValidFrom <= now && p.ValidTo >= now)
        .ToList();

    foreach (var line in dto.Lines)
    {
        // -----------------------------
        // TERMÉK ÉRTÉKESÍTÉS
        // -----------------------------
        if (line.ProductId != null)
        {
            var product = await _context.Products
                .Include(p => p.ProductGroup)
                .FirstOrDefaultAsync(p => p.Id == line.ProductId);

            if (product == null)
                return BadRequest("Product not found");

            if (product.StockQuantity < line.Quantity)
                return BadRequest($"Not enough stock for product {product.Name}");

            // Akciók alkalmazása
            var price = ApplyPromotions(product, activePromotions);

            // ⭐ VIP kedvezmény alkalmazása
            if (partner.IsVip)
            {
                price = price - (price * partner.VipDiscountPercent / 100m);
            }

            // Készlet csökkentése
            product.StockQuantity -= line.Quantity;

            sale.Lines.Add(new SaleLine
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = price
            });

            total += price * line.Quantity;
        }

        // -----------------------------
        // CSOMAG ÉRTÉKESÍTÉS
        // -----------------------------
        else if (line.PackageId != null)
        {
            var package = await _context.Packages
                .Include(p => p.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.ProductGroup)
                .FirstOrDefaultAsync(p => p.Id == line.PackageId);

            if (package == null)
                return BadRequest("Package not found");

            foreach (var item in package.Items)
            {
                var requiredQty = item.Quantity * line.Quantity;

                if (item.Product.StockQuantity < requiredQty)
                    return BadRequest($"Not enough stock for product {item.Product.Name}");

                // Akciók alkalmazása
                var price = ApplyPromotions(item.Product, activePromotions);

                // ⭐ VIP kedvezmény alkalmazása
                if (partner.IsVip)
                {
                    price = price - (price * partner.VipDiscountPercent / 100m);
                }

                // Készlet csökkentése
                item.Product.StockQuantity -= requiredQty;

                sale.Lines.Add(new SaleLine
                {
                    ProductId = item.ProductId,
                    Quantity = requiredQty,
                    UnitPrice = price
                });

                total += price * requiredQty;
            }
        }
    }

    sale.TotalAmount = total;

    _context.Sales.Add(sale);
    await _context.SaveChangesAsync();

    return Ok(sale);
}

private decimal ApplyPromotions(Product product, List<Promotion> promotions)
{
    decimal basePrice = product.BasePrice;

    var productPromo = promotions
        .Where(p => p.PromotionProducts.Any(pp => pp.ProductId == product.Id))
        .FirstOrDefault();

    var groupPromo = promotions
        .Where(p => product.ProductGroupId != null &&
                    p.PromotionGroups.Any(pg => pg.ProductGroupId == product.ProductGroupId))
        .FirstOrDefault();

    var discount = new[]
    {
        productPromo?.DiscountPercent ?? 0,
        groupPromo?.DiscountPercent ?? 0
    }.Max();

    return basePrice * (1 - discount / 100m);
}




  /*[HttpPost]
  public async Task<IActionResult> CreateSale(CreateSaleDto dto)
  {
      var partner = await _context.Partners.FindAsync(dto.PartnerId);
      if (partner == null)
          return BadRequest("Partner not found");

      var sale = new Sale
      {
          PartnerId = dto.PartnerId,
          Date = DateTime.UtcNow
      };

      decimal total = 0;
      var now = DateTime.UtcNow;

      // Érvényes akciók lekérése
      var promotions = await _context.Promotions
          .Include(p => p.PromotionProducts)
          .Include(p => p.PromotionGroups)
          .ToListAsync();

      var activePromotions = promotions
          .Where(p => p.ValidFrom <= now && p.ValidTo >= now)
          .ToList();

      foreach (var line in dto.Lines)
      {
          // -----------------------------
          // TERMÉK ÉRTÉKESÍTÉS
          // -----------------------------
          if (line.ProductId != null)
          {
              var product = await _context.Products
                  .Include(p => p.ProductGroup)
                  .FirstOrDefaultAsync(p => p.Id == line.ProductId);

              if (product == null)
                  return BadRequest("Product not found");

              if (product.StockQuantity < line.Quantity)
                  return BadRequest($"Not enough stock for product {product.Name}");

              // Akciók alkalmazása
              var price = ApplyPromotions(product, activePromotions);

              // Készlet csökkentése
              product.StockQuantity -= line.Quantity;

              sale.Lines.Add(new SaleLine
              {
                  ProductId = product.Id,
                  Quantity = line.Quantity,
                  UnitPrice = price
              });

              total += price * line.Quantity;
          }

          // -----------------------------
          // CSOMAG ÉRTÉKESÍTÉS
          // -----------------------------
          else if (line.PackageId != null)
          {
              var package = await _context.Packages
                  .Include(p => p.Items)
                  .ThenInclude(i => i.Product)
                  .ThenInclude(p => p.ProductGroup)
                  .FirstOrDefaultAsync(p => p.Id == line.PackageId);

              if (package == null)
                  return BadRequest("Package not found");

              foreach (var item in package.Items)
              {
                  var requiredQty = item.Quantity * line.Quantity;

                  if (item.Product.StockQuantity < requiredQty)
                      return BadRequest($"Not enough stock for product {item.Product.Name}");

                  // Akciók alkalmazása
                  var price = ApplyPromotions(item.Product, activePromotions);

                  // Készlet csökkentése
                  item.Product.StockQuantity -= requiredQty;

                  sale.Lines.Add(new SaleLine
                  {
                      ProductId = item.ProductId,
                      Quantity = requiredQty,
                      UnitPrice = price
                  });

                  total += price * requiredQty;
              }
          }
      }

      sale.TotalAmount = total;

      _context.Sales.Add(sale);
      await _context.SaveChangesAsync();

      return Ok(sale);
  }

  // ---------------------------------------------------------
  // AKCIÓK ALKALMAZÁSA (termékre és termékcsoportra)
  // ---------------------------------------------------------
  private decimal ApplyPromotions(Product product, List<Promotion> promotions)
  {
      decimal basePrice = product.BasePrice;

      var productPromo = promotions
          .Where(p => p.PromotionProducts.Any(pp => pp.ProductId == product.Id))
          .FirstOrDefault();

      var groupPromo = promotions
          .Where(p => product.ProductGroupId != null &&
                      p.PromotionGroups.Any(pg => pg.ProductGroupId == product.ProductGroupId))
          .FirstOrDefault();

      var discount = new[]
      {
          productPromo?.DiscountPercent ?? 0,
          groupPromo?.DiscountPercent ?? 0
      }.Max();

      return basePrice * (1 - discount / 100m);
  }*/
}