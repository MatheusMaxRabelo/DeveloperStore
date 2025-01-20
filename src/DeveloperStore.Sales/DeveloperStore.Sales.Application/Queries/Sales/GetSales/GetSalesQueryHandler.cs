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
        request.Filters.TryGetValue(Constants.Filter.PAGE_NUMBER_KEY, out string? pageNumberValue);
        request.Filters.TryGetValue(Constants.Filter.PAGE_SIZE_KEY, out string? pageSizeValue);

        var sales = await salesService.GetSalesAsync(request.Filters.ToDictionary());

        var pageNumber = pageNumberValue is null ? 1 : int.Parse(pageNumberValue);

        var pageSize = pageSizeValue is null ? 10 : int.Parse(pageSizeValue);
        var result = new PagedResponse<List<SalesDto>>(
            mapper.Map<List<SalesDto>>(sales),
            1, 10, 10);

        return result;
    }
}
