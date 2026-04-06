public class UpdateProductDto
{
    public required string Name { get; set; }
    public decimal BasePrice { get; set; }
    public int? ProductGroupId { get; set; }
    public int AddStock { get; set; }  // +5, +10, stb.
}
