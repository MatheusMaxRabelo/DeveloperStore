using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.Sales.Create;

public record CreateSaleCommand : IRequest<SalesDto>
{
    public int CustomerId { get; set; }
    public string Branch { get; set; } = string.Empty;
    public List<ProductRequest> Products { get; set; }
}

public record ProductRequest(int ProductId, int Quantity);