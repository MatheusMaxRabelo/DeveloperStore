using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Castle.Core.Logging;
using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Application.Mappers;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Events.Sales;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using DeveloperStore.Sales.Domain.Models;
using DeveloperStore.Sales.Application.Commands.Sales.Update;
using DeveloperStore.Sales.Application.Models;

namespace DeveloperStore.Sales.UnitTests.Application.Handlers;

public class UpdateSaleCommandHandlerTests
{
    private readonly Faker _faker;
    private readonly ISalesService _salesService;
    private readonly IDomainEventService _domainEventService;
    private readonly ILogger<CreateSaleCommandHandler> _logger;
    private readonly UpdateSaleCommandHandler _handler;
    public UpdateSaleCommandHandlerTests()
    {
        _faker = new Faker();
        _salesService = Substitute.For<ISalesService>();
        _domainEventService = Substitute.For<IDomainEventService>();
        _logger = Substitute.For<ILogger<CreateSaleCommandHandler>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new SalesMapperProfile()));
        var mapper = config.CreateMapper();

        _handler = new UpdateSaleCommandHandler(_salesService, mapper, _domainEventService);
    }

    [Fact]
    public async Task Handle_ShouldUpdateSale_AndReturnSalesDto()
    {
        // Arrange
        var saleId = _faker.Random.Int(1, 10);
        var customerId = _faker.Random.Int(1,10);
        var branch = _faker.Company.CompanyName();
        var products = new List<UpdateProductRequest>
        {
            new UpdateProductRequest { Id = _faker.Random.Int(1, 10), Quantity = _faker.Random.Int(1, 10) },
            new UpdateProductRequest { Id = _faker.Random.Int(1, 10), Quantity = _faker.Random.Int(1, 10) },
        };

        var updateSaleCommand = new UpdateSaleCommand
        {
            Id = saleId,
            CustomerId = customerId,
            Branch = branch,
            Products = products,
            IsCancelled = false
        };

        var updatedSale = new SalesModel
        {
            Id = saleId,
            Customer = new CustomerModel { Id = customerId },
            Branch = branch,
            IsCancelled = false,
            TotalAmount = 100,
            SalesDate = _faker.Date.Past()
        };

        _salesService.UpdateSaleAsync(Arg.Any<int>(), Arg.Any<Sale>()).Returns(updatedSale);

        _domainEventService.PublishAsync(Arg.Any<IReadOnlyCollection<DomainEvent>>()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(updateSaleCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Id);
        Assert.Equal(branch, result.Branch);
        Assert.Equal(100, result.TotalAmount);

        await _salesService.Received(1).UpdateSaleAsync(saleId, Arg.Any<Sale>());

        await _domainEventService.Received(1).PublishAsync(Arg.Any<List<DomainEvent>>());
    }
}
