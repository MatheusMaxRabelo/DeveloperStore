using System.Text.Json.Serialization;
using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.Sales.Update;

public class UpdateSaleCommand : IRequest<SalesDto>
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("salesDate")]
    public DateTime SalesDate { get; set; }

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

    [JsonPropertyName("totalAmount")]
    public double TotalAmount { get; set; }

    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    [JsonPropertyName("products")]
    public List<UpdateProductRequest> Products { get; set; }

    [JsonPropertyName("isCancelled")]
    public bool IsCancelled { get; set; }
}
public record UpdateProductRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; } = 0m;

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("discount")]
    public decimal Discount { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("isCancelled")]
    public bool IsCancelled { get; set; } = false;
}