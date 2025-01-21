using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.Sales.Update;

public class UpdateSaleCommand : IRequest<SalesDto>
{
    [NotMapped]
    public int Id { get; set; }

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

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

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("isCancelled")]
    public bool IsCancelled { get; set; } = false;
}