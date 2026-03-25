public class Sale
{
    public int Id { get; set; }
    public int PartnerId { get; set; }
    public Partner Partner { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public ICollection<SaleLine> Lines { get; set; } = new List<SaleLine>();
}
