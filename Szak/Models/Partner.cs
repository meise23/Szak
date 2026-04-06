public class Partner
{
  public int Id{get;set;}
  public string Name {get;set;}=string.Empty;
  public string TaxNumber {get;set;}=string.Empty;
  public string Email {get;set;}=string.Empty;
  public bool IsVip { get; set; } = false;
  public int VipDiscountPercent { get; set; } = 0; // pl. 5
}