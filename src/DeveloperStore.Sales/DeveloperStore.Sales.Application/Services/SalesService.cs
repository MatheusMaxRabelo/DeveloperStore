using AutoMapper;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Services;

public class SalesService : ISalesService
{
    private readonly ISalesRepository _salesRepository;
    private readonly ILogger _logger;

    public const int MaxQuantity = 20;
    public const int MinDiscountQuantity = 4;

    public SalesService(ISalesRepository salesRepository, ILogger<SalesService> logger)
    {
        _salesRepository = salesRepository;
        _logger = logger;
    }

    public async Task<List<Sale>> GetSalesAsync(int pageNumber, int pageSize)
    {
        return await _salesRepository.GetSalesAsync(pageNumber, pageSize);
    }

    public async Task<Sale> GetSaleByIdAsync(int id)
    {
        var sale = await _salesRepository.GetSaleByIdAsync(id);
        if (sale == null) throw new KeyNotFoundException("Sale not found");
        return sale;
    }

    public async Task<Sale> CreateSaleAsync(Sale sale)
    {
        if (sale.Items == null || !sale.Items.Any())
            throw new ArgumentException("A sale must have at least one item");

        //TODO: Retrieve data from API to calculate price/total amount

        var tasks = sale.Items.Select(async item =>
        {
            try
            {
                decimal discountedPrice = await CalculateDiscountedPriceAsync(item);
                _logger.LogInformation($"Quantity: {item.Quantity}, Total Price: {discountedPrice:C}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error for Quantity {item.Quantity}: {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);

        sale.TotalAmount = sale.Items.Sum(x=> x.TotalAmount);

        await _salesRepository.AddAsync(sale);

        return sale;
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
            if (item.Quantity > MaxQuantity)
            {
                throw new InvalidOperationException($"Cannot purchase more than {MaxQuantity} identical items.");
            }

            decimal discount = GetDiscountRate(item.Quantity);
            item.Discount = discount;
            decimal totalPrice = item.Quantity * item.UnitPrice;
            item.TotalAmount = totalPrice - (totalPrice * discount);

            return item.TotalAmount;
        });
    }

    private decimal GetDiscountRate(int quantity)
    {
        if (quantity >= 10 && quantity <= MaxQuantity)
        {
            return 0.20m; // 20% discount
        }
        else if (quantity >= MinDiscountQuantity)
        {
            return 0.10m; // 10% discount
        }

        return 0.0m; // No discount
    }

    private bool IsDiscountApplicable(int quantity)
    {
        return quantity >= MinDiscountQuantity && quantity <= MaxQuantity;
    }
}
