using AutoMapper;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Application.Response;
using DeveloperStore.Sales.Domain.Events.Sales;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Commands.Sales.Delete;

public class DeleteSaleCommandHandler(ISalesService salesService, IMapper mapper, ILogger<DeleteSaleCommand> logger, IDomainEventService domainEventService)
    : IRequestHandler<DeleteSaleCommand, SimpleResponse>
{
    public async Task<SimpleResponse> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var result = await salesService.DeleteSaleAsync(request.Id);

        await domainEventService.PublishAsync(new SaleCancelled(request.Id));

        return new SimpleResponse() { Message = result };
    }
}
