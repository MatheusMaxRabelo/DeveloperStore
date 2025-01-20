namespace DeveloperStore.Sales.Domain.Models;

public class SalesModel
{
    public int Id { get; set; }
    public DateTime SalesDate { get; set; }
    public CustomerModel Customer { get; set; }
    public double TotalAmount { get; set; }
    public string Branch { get; set; } = null!;
    public List<ProductModel> Products { get; set; } = null!;
    public bool IsCancelled { get; set; } = false;
}
