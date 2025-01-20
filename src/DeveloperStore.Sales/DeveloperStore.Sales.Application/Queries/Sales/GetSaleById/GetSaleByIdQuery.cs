using DeveloperStore.Sales.Application.DTOs;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.Sales.GetSaleById;

public class GetSaleByIdQuery : IRequest<SalesDto>
{
    public int Id { get; set; }

    public GetSaleByIdQuery(int id)
    {
        Id = id;
    }
}
