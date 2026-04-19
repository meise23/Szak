public class ProductGroupItem
{
    public int Id { get; set; }

    public int ProductGroupId { get; set; }
    public ProductGroup? ProductGroup { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
}
