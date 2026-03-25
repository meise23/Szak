public class CreatePromotionDto
{
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public List<int> ProductIds { get; set; } = new();
    public List<int> ProductGroupIds { get; set; } = new();
}
