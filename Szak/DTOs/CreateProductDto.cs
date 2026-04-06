public class CreateProductDto
{
    public required string Name { get; set; }
    public decimal BasePrice { get; set; }
    public int? ProductGroupId { get; set; }
    public int StockQuantity { get; set; }
}
