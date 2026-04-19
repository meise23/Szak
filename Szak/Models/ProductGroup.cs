public class ProductGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductGroupItem> Items { get; set; } = new List<ProductGroupItem>();
}
