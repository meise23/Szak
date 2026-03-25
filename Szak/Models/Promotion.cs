public class Promotion
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal DiscountPercent { get; set; }

    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
    public ICollection<PromotionGroup> PromotionGroups { get; set; } = new List<PromotionGroup>();
}
