using AutoMapper;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Queries.Sales.GetSaleById;

public class GetSaleByIdQueryHandler(ISalesService salesService, IMapper mapper, ILogger<GetSaleByIdQueryHandler> logger) : IRequestHandler<GetSaleByIdQuery, SalesDto>
{
    public async Task<SalesDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await salesService.GetSaleByIdAsync(request.Id);

        return mapper.Map<SalesDto>(sale);
    }
}
