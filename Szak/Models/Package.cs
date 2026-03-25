public class Package
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<PackageItem> Items { get; set; } = new List<PackageItem>();
}
