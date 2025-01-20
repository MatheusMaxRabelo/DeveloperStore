using AutoMapper;
using DeveloperStore.Sales.Application.Response;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using DeveloperStore.Sales.Application.DTOs;

namespace DeveloperStore.Sales.Application.Queries.Sales.GetSales;

public class GetSalesQueryHandler(ISalesService salesService, IMapper mapper, ILogger<GetSalesQueryHandler> logger) : IRequestHandler<GetSalesQuery, PagedResponse<List<SalesDto>>>
{
    public async Task<PagedResponse<List<SalesDto>>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await salesService.GetSalesAsync(request.Filters.ToDictionary());

        var result = new PagedResponse<List<SalesDto>>(
            mapper.Map<List<SalesDto>>(sales),
            1, 10, 10);

        return result;
    }
}
