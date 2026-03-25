public class PromotionGroup
{
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; } = null!;

    public int ProductGroupId { get; set; }
    public ProductGroup ProductGroup { get; set; } = null!;
}
