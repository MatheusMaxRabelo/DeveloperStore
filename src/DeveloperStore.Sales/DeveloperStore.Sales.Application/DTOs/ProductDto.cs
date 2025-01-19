namespace DeveloperStore.Sales.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } = 0m;
    public string Image { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; } = false;
}
