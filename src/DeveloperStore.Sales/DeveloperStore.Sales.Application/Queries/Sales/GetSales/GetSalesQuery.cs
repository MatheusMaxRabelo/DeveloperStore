using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Response;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.Sales.GetSales;

public class GetSalesQuery : IRequest<PagedResponse<List<SalesDto>>>
{
    public IDictionary<string, string> Filters { get; set; }
}
