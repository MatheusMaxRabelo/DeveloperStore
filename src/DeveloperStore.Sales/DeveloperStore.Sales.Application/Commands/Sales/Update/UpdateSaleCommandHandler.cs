using AutoMapper;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Commands.Sales.Update;

public class UpdateSaleCommandHandler(ISalesService salesService, IMapper mapper, ILogger<UpdateSaleCommandHandler> logger) : IRequestHandler<UpdateSaleCommand, SalesDto>
{
    public async Task<SalesDto> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = mapper.Map<Sale>(request);

        var result = await salesService.UpdateSaleAsync(request.Id, sale);

        return mapper.Map<SalesDto>(result);
    }
}
