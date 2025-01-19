using AutoMapper;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Commands.Sales.Create;

public class CreateSaleCommandHandler(IMapper mapper, ILogger<CreateSaleCommandHandler> logger, ISalesService salesService) : IRequestHandler<CreateSaleCommand, SalesDto>
{
    public const int MaxQuantity = 20;
    public const int MinDiscountQuantity = 4;

    public async Task<SalesDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating sale!");

        var dto = mapper.Map<SalesDto>(request);
        var sale = mapper.Map<Sale>(request);
        sale.SalesDate = DateTime.UtcNow;

        var result = mapper.Map<SalesDto>(await salesService.CreateSaleAsync(sale));

        return result;
    }

}
