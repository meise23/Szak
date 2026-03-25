public class CreateSaleDto
{
    public int PartnerId { get; set; }
    public List<CreateSaleLineDto> Lines { get; set; } = new();
}

public class CreateSaleLineDto
{
    public int? ProductId { get; set; }
    public int? PackageId { get; set; }
    public int Quantity { get; set; }
}
