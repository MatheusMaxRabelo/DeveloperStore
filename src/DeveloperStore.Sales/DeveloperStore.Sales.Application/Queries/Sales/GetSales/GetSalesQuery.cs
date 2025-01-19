using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Response;
using MediatR;

namespace DeveloperStore.Sales.Application.Queries.Sales.GetSales;

public class GetSalesQuery : BaseQueryRequest, IRequest<PagedResponse<List<SalesDto>>>
{
}
