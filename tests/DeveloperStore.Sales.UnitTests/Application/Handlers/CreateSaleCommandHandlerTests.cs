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

namespace DeveloperStore.Sales.UnitTests.Application.Handlers;

public class CreateSaleCommandHandlerTests
{
    private readonly Faker _faker;
    private readonly ISalesService _salesService;
    private readonly IDomainEventService _domainEventService;
    private readonly ILogger<CreateSaleCommandHandler> _logger;
    private readonly CreateSaleCommandHandler _handler;
    public CreateSaleCommandHandlerTests()
    {
        _faker = new Faker();
        _salesService = Substitute.For<ISalesService>();
        _domainEventService = Substitute.For<IDomainEventService>();
        _logger = Substitute.For<ILogger<CreateSaleCommandHandler>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new SalesMapperProfile()));
        var mapper = config.CreateMapper();

        _handler = new CreateSaleCommandHandler(mapper, _logger, _salesService, _domainEventService);
    }

    [Fact]
    public async Task Handle_ShouldCreateSale_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateSaleCommand()
        {
            Branch = _faker.Company.Locale,
            CustomerId = _faker.Random.Int(1, 10),
            Products = new List<ProductRequest>()
            {
                new ProductRequest(_faker.Random.Int(1, 10), _faker.Random.Int(1, 10)),
                new ProductRequest(_faker.Random.Int(1, 10), _faker.Random.Int(1, 10)),
                new ProductRequest(_faker.Random.Int(1, 10), _faker.Random.Int(1, 10)),
                new ProductRequest(_faker.Random.Int(1, 10), _faker.Random.Int(1, 10)),
            }
        };

        var sale = new SalesModel()
        {
            Id = _faker.Random.Int(1, 10),
            SalesDate = DateTime.UtcNow,
            Customer = new CustomerModel { Id = command.CustomerId },
            TotalAmount = _faker.Random.Double(10, 100),
            IsCancelled = false,
        };

        var expectedDto = new SalesDto()
        {
            Id = sale.Id,
            SalesDate = sale.SalesDate,
            Customer = new CustomerDto { Id = sale.Customer.Id },
            TotalAmount = (double)sale.TotalAmount,
            IsCancelled = sale.IsCancelled,
        };

        _salesService.CreateSaleAsync(Arg.Any<Sale>()).Returns(sale);
        _domainEventService.PublishAsync(Arg.Any<IReadOnlyCollection<DomainEvent>>()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.SalesDate, result.SalesDate);
        Assert.Equal(expectedDto.Customer.Id, result.Customer.Id);
        Assert.Equal(expectedDto.TotalAmount, result.TotalAmount);
        Assert.Equal(expectedDto.IsCancelled, result.IsCancelled);

        await _salesService.Received(1).CreateSaleAsync(Arg.Is<Sale>(s =>
            s.CustomerId == command.CustomerId &&
            s.Items.Count == command.Products.Count &&
            s.SalesDate.Date == DateTime.UtcNow.Date
        ));

        await _domainEventService.Received(1).PublishAsync(Arg.Any<List<DomainEvent>>());
    }
}
