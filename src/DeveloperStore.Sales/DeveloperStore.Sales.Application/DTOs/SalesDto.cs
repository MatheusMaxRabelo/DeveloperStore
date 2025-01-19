namespace DeveloperStore.Sales.Application.DTOs;

public class SalesDto
{
    public int Id { get; set; }
    public DateTime SalesDate { get; set; }
    public CustomerDto Customer { get; set; }
    public double TotalAmount { get; set; }
    public string Branch { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = null!;
    public bool IsCancelled { get; set; } = false;
}
