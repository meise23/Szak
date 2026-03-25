using Microsoft.AspNetCore.Mvc;

[ApiController]

[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PromotionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePromotionDto dto)
    {
        var promotion = new Promotion
        {
            Name = dto.Name,
            DiscountPercent = dto.DiscountPercent,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo
        };

        foreach (var productId in dto.ProductIds)
        {
            promotion.PromotionProducts.Add(new PromotionProduct
            {
                ProductId = productId
            });
        }

        foreach (var groupId in dto.ProductGroupIds)
        {
            promotion.PromotionGroups.Add(new PromotionGroup
            {
                ProductGroupId = groupId
            });
        }

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return Ok(promotion);
    }
}
