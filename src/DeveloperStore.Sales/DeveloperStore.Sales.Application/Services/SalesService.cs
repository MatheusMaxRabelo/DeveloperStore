﻿using AutoMapper;
using DeveloperStore.Sales.Application.Models;
using DeveloperStore.Sales.Application.Services.Refit;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using DeveloperStore.Sales.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Services;

public class SalesService : ISalesService
{
    private readonly ISalesRepository _salesRepository;
    private readonly ILogger _logger;
    private readonly ICustomerApi _customerApi;
    private readonly IProductApi _productApi;
    private readonly IMapper _mapper;

    public const int MAX_QUANTITY = 20;
    public const int MIN_DISCOUNT_QUANTITY = 4;
    public const string PAGE_NUMBER_KEY = "_page"; 
    public const string PAGE_SIZE_KEY = "_size";

    public SalesService(ISalesRepository salesRepository, ILogger<SalesService> logger, ICustomerApi customerApi, IProductApi productApi, IMapper mapper)
    {
        _salesRepository = salesRepository;
        _logger = logger;
        _customerApi = customerApi;
        _productApi = productApi;
        _mapper = mapper;
    }

    public async Task<List<SalesModel>> GetSalesAsync(Dictionary<string, string> filters)
    {
        filters.TryGetValue(PAGE_NUMBER_KEY, out string? pageNumberValue);
        filters.TryGetValue(PAGE_SIZE_KEY, out string? pageSizeValue);

        var pageNumber = pageNumberValue is null ? 1 : int.Parse(pageNumberValue);
        var pageSize = pageSizeValue is null ? 10 : int.Parse(pageSizeValue);

        if (!string.IsNullOrWhiteSpace(pageNumberValue))
        {
            filters.Remove(PAGE_NUMBER_KEY);
        }

        if (!string.IsNullOrWhiteSpace(pageSizeValue))
        {
            filters.Remove(PAGE_SIZE_KEY);
        }

        var (sales, totalamount) = await _salesRepository.GetSalesAsync(pageNumber, pageSize, filters);

        var salesList = _mapper.Map<List<SalesModel>>(sales);

        var saleTasks = salesList.Select(async sale =>
        {
            await FulfillProductData(sale, string.Empty);
            await FulfillCustomerData(sale);
        });

        await Task.WhenAll(saleTasks);

        return salesList;
    }

    public async Task<SalesModel> GetSaleByIdAsync(int id)
    {
        var sale = await _salesRepository.GetSaleByIdAsync(id);
        if (sale == null) throw new KeyNotFoundException("Sale not found");

        return _mapper.Map<SalesModel>(sale);
    }

    public async Task<SalesModel> CreateSaleAsync(Sale sale)
    {
        if (sale.Items == null || !sale.Items.Any())
            throw new ArgumentException("A sale must have at least one item");

        var tasks = sale.Items.Select(async item =>
        {
            try
            {
                var product = await _productApi.GetProductAsync(item.ItemId);

                _mapper.Map(product, item);

                decimal discountedPrice = await CalculateDiscountedPriceAsync(item);
                _logger.LogInformation($"Quantity: {item.Quantity}, Total Price: {discountedPrice:C}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error for Quantity {item.Quantity}: {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);

        sale.TotalAmount = sale.Items.Sum(x => x.TotalAmount);

        await _salesRepository.AddAsync(sale);

        return _mapper.Map<SalesModel>(sale);
    }

    public async Task UpdateSaleAsync(int id, Sale sale)
    {
        var existingSale = await _salesRepository.GetSaleByIdAsync(id);
        if (existingSale == null) throw new KeyNotFoundException("Sale not found");

        existingSale.SalesDate = sale.SalesDate;
        existingSale.CustomerId = sale.CustomerId;
        existingSale.Branch = sale.Branch;
        existingSale.TotalAmount = sale.TotalAmount;
        existingSale.IsCancelled = sale.IsCancelled;

        await _salesRepository.UpdateAsync(existingSale);
    }

    public async Task DeleteSaleAsync(int id)
    {
        var sale = await _salesRepository.GetSaleByIdAsync(id);
        if (sale == null) throw new KeyNotFoundException("Sale not found");

        await _salesRepository.DeleteAsync(id);
    }

    private Task<decimal> CalculateDiscountedPriceAsync(Item item)
    {
        return Task.Run(() =>
        {
            if (item.Quantity > MAX_QUANTITY)
            {
                throw new InvalidOperationException($"Cannot purchase more than {MAX_QUANTITY} identical items.");
            }

            if (IsDiscountApplicable(item.Quantity))
            {
                decimal discount = GetDiscountRate(item.Quantity);
                item.Discount = discount;
            }

            decimal totalPrice = item.Quantity * item.UnitPrice;
            item.TotalAmount = Decimal.Round(totalPrice - (totalPrice * item.Discount), 2);

            return item.TotalAmount;
        });
    }

    private decimal GetDiscountRate(int quantity)
    {
        if (quantity >= 10 && quantity <= MAX_QUANTITY)
        {
            return 0.20m;
        }
        else if (quantity >= MIN_DISCOUNT_QUANTITY)
        {
            return 0.10m;
        }

        return 0.0m;
    }

    private bool IsDiscountApplicable(int quantity)
    {
        return quantity >= MIN_DISCOUNT_QUANTITY && quantity <= MAX_QUANTITY;
    }

    private void ProcessCustomer(CustomerModel saleCustomer, Customer customer)
    {
        if (customer is null)
        {
            return;
        }

        _mapper.Map(customer, saleCustomer);
    }

    private void ProcessProduct(ProductModel saleProduct, Product product, string action)
    {
        if (product is null)
        {
            return;
        }

        _mapper.Map(product, saleProduct);

        if (!string.IsNullOrWhiteSpace(action) && action == "creating")
        {
            saleProduct.UnitPrice = product.Price;
        }
    }

    private async Task FulfillProductData(SalesModel sale, string action)
    {
        var productTasks = sale.Products.Select(async item =>
        {
            var product = await _productApi.GetProductAsync(item.Id);

            ProcessProduct(item, product, action);

            return product;
        });

        await Task.WhenAll(productTasks);
    }

    private async Task FulfillCustomerData(SalesModel sale)
    {
        var customer = await _customerApi.GetCustomerAsync(sale.Customer.Id);

        ProcessCustomer(sale.Customer, customer);
    }
}
