public class Product
{
  public int Id {get;set;}
  public string Name {get;set;}=string.Empty;
  public string Sku {get;set;}=string.Empty;
  public decimal BasePrice {get;set;}
public int StockQuantity {get;set;}

public int? ProductGroupId { get; set; }
public ProductGroup? ProductGroup { get; set; }

}