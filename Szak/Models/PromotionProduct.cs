public class PromotionProduct
{
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
