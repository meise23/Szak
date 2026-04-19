public class CreateProductGroupDto
{
    public string Name { get; set; } ="";
    public List<CreateProductGroupItemDto> Items { get; set; } = new ();
}

public class CreateProductGroupItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

