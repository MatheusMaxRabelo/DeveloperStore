using System.Text.Json.Serialization;

namespace DeveloperStore.Sales.Application.DTOs;

public class ProductDto
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
