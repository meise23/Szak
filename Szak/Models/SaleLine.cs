public class SaleLine
{
    public int Id { get; set; }

    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;

    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public int? PackageId { get; set; }
    public Package? Package { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
