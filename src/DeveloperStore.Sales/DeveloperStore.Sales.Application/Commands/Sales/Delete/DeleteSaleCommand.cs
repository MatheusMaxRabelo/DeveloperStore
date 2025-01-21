using DeveloperStore.Sales.Application.Response;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.Sales.Delete;

public class DeleteSaleCommand : IRequest<SimpleResponse>
{
    public int Id { get; set; }
}
