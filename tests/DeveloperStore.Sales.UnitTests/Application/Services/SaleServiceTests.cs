using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperStore.Sales.Application.Exceptions;
using DeveloperStore.Sales.Application.Services.Refit;
using DeveloperStore.Sales.Application.Services;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using DeveloperStore.Sales.Application.Mappers;
using DeveloperStore.Sales.Application.Models;

namespace DeveloperStore.Sales.UnitTests.Application.Services;

public class SalesServiceTests
{
    private readonly ISalesRepository _salesRepository;
    private readonly ICustomerApi _customerApi;
    private readonly IProductApi _productApi;
    private readonly ILogger<SalesService> _logger;
    private readonly IMapper _mapper;
    private readonly SalesService _salesService;

    public SalesServiceTests()
    {
        _salesRepository = Substitute.For<ISalesRepository>();
        _customerApi = Substitute.For<ICustomerApi>();
        _productApi = Substitute.For<IProductApi>();
        _logger = Substitute.For<ILogger<SalesService>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new SalesMapperProfile()));
        _mapper = config.CreateMapper();
        _salesService = new SalesService(_salesRepository, _logger, _customerApi, _productApi, _mapper);
    }

    [Fact]
    public async Task GetSalesAsync_ShouldReturnSalesWithValidFilters()
    {
        // Arrange
        var filters = new Dictionary<string, string>
            {
                { "pageNumber", "1" },
                { "pageSize", "10" }
            };

        var sales = new List<Sale>
            {
                new Sale { Id = 1, SalesDate = DateTime.Now, TotalAmount = 100, IsCancelled = false },
                new Sale { Id = 2, SalesDate = DateTime.Now, TotalAmount = 200, IsCancelled = false }
            };

        // Mock repository call
        _salesRepository.GetSalesAsync(1, 10, filters).Returns((sales, 2));

        // Mock API calls
        _productApi.GetProductAsync(Arg.Any<int>()).Returns(new Product { Id = 1, Title = "Product1", Price = 50 });
        _customerApi.GetCustomerAsync(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = new Name { Firstname = "Customer1" } });

        // Act
        var (resultSales, totalItems) = await _salesService.GetSalesAsync(filters);

        // Assert
        Assert.Equal(2, resultSales.Count);
        Assert.Equal(2, totalItems);
    }

    [Fact]
    public async Task GetSaleByIdAsync_ShouldReturnSale_WhenSaleExists()
    {
        // Arrange
        var saleId = 1;
        var sale = new Sale { Id = saleId, SalesDate = DateTime.Now, TotalAmount = 100, IsCancelled = false };

        // Mock repository call
        _salesRepository.GetSaleByIdAsync(saleId).Returns(sale);

        // Mock API calls
        _productApi.GetProductAsync(Arg.Any<int>()).Returns(new Product { Id = 1, Title = "Product1", Price = 50 });
        _customerApi.GetCustomerAsync(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = new Name { Firstname = "Customer1" } });

        // Act
        var result = await _salesService.GetSaleByIdAsync(saleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Id);
    }

    [Fact]
    public async Task CreateSaleAsync_ShouldCreateSale_WhenValidData()
    {
        // Arrange
        var sale = new Sale
        {
            SalesDate = DateTime.Now,
            TotalAmount = 100,
            Items = new List<Item>
                {
                    new Item { ItemId = 1, Quantity = 2, UnitPrice = 50 }
                }
        };

        _salesRepository.AddAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

        _productApi.GetProductAsync(Arg.Any<int>()).Returns(new Product { Id = 1, Title = "Product1", Price = 50 });
        _customerApi.GetCustomerAsync(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = new Name { Firstname = "Customer1" } });

        // Act
        var result = await _salesService.CreateSaleAsync(sale);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((double)sale.TotalAmount, result.TotalAmount);
    }

    [Fact]
    public async Task UpdateSaleAsync_ShouldUpdateSale_WhenSaleExists()
    {
        // Arrange
        var saleId = 1;
        var existingSale = new Sale
        {
            Id = saleId,
            SalesDate = DateTime.Now,
            CustomerId = 1,
            Branch = "Branch1",
            TotalAmount = 100,
            Items = new List<Item> { new Item { ItemId = 1, Quantity = 2, UnitPrice = 50 } },
            IsCancelled = false
        };

        var updatedSale = new Sale
        {
            Id = saleId,
            SalesDate = DateTime.Now,
            CustomerId = 1,
            Branch = "Branch2",
            TotalAmount = 200,
            Items = new List<Item> { new Item { ItemId = 2, Quantity = 4, UnitPrice = 50 } },
            IsCancelled = false
        };

        _salesRepository.GetSaleByIdAsync(saleId).Returns(existingSale);
        _salesRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

        _productApi.GetProductAsync(Arg.Any<int>()).Returns(new Product { Id = 2, Title = "Product2", Price = 50 });
        _customerApi.GetCustomerAsync(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = new Name { Firstname = "Customer1" } });

        // Act
        var result = await _salesService.UpdateSaleAsync(saleId, updatedSale);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Branch2", result.Branch);
        Assert.Equal(180, result.TotalAmount);
    }

    [Fact]
    public async Task DeleteSaleAsync_ShouldDeleteSale_WhenSaleExists()
    {
        // Arrange
        var saleId = 1;
        var sale = new Sale { Id = saleId, IsCancelled = false };

        // Mock repository calls
        _salesRepository.GetSaleByIdAsync(saleId).Returns(sale);
        _salesRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

        // Act
        var result = await _salesService.DeleteSaleAsync(saleId);

        // Assert
        Assert.Equal("Sale successfully deleted", result);
        Assert.True(sale.IsCancelled);
    }
}