using AutoMapper;
using DeveloperStore.Sales.Application.Exceptions;
using DeveloperStore.Sales.Application.Models;
using DeveloperStore.Sales.Application.Resources;
using DeveloperStore.Sales.Application.Response;
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
        filters.TryGetValue(Constants.Filter.PAGE_NUMBER_KEY, out string? pageNumberValue);
        filters.TryGetValue(Constants.Filter.PAGE_SIZE_KEY, out string? pageSizeValue);

        var pageNumber = pageNumberValue is null ? 1 : int.Parse(pageNumberValue);
        var pageSize = pageSizeValue is null ? 10 : int.Parse(pageSizeValue);

        if (!string.IsNullOrWhiteSpace(pageNumberValue))
        {
            filters.Remove(Constants.Filter.PAGE_NUMBER_KEY);
        }

        if (!string.IsNullOrWhiteSpace(pageSizeValue))
        {
            filters.Remove(Constants.Filter.PAGE_SIZE_KEY);
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

        if (sale == null)
        {
            throw new BusinessException(new ErrorResponse()
            {
                Type = Errors.ResourceNotFoundType,
                Error = Errors.SaleNotFoundMessage,
                Detail = string.Format(Errors.SaleNotFoundDetail, id)
            });
        }

        var result = _mapper.Map<SalesModel>(sale);
        await FulfillProductData(result, string.Empty);
        await FulfillCustomerData(result);

        return result;
    }

    public async Task<SalesModel> CreateSaleAsync(Sale sale)
    {
        if (sale.Items == null || !sale.Items.Any())
        {
            throw new BusinessException(new ErrorResponse
            {
                Type = Errors.NoItemsErrorType,
                Error = Errors.NoItemsErrorMessage,
                Detail = Errors.NoItemsErrorDetail
            });
        }
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

        var result = _mapper.Map<SalesModel>(sale);

        await FulfillProductData(result, "creating");
        await FulfillCustomerData(result);

        sale = _mapper.Map<Sale>(sale);

        await _salesRepository.AddAsync(sale);

        return result;
    }

    public async Task<SalesModel> UpdateSaleAsync(int id, Sale sale)
    {
        var existingSale = await _salesRepository.GetSaleByIdAsync(id);

        if (existingSale == null)
        {
            throw new BusinessException(new ErrorResponse()
            {
                Type = Errors.ResourceNotFoundType,
                Error = Errors.SaleNotFoundMessage,
                Detail = string.Format(Errors.SaleNotFoundDetail, id)
            });
        }

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

        existingSale.SalesDate = sale.SalesDate.ToUniversalTime();
        existingSale.CustomerId = sale.CustomerId;
        existingSale.Branch = sale.Branch;
        existingSale.Items = sale.Items;
        existingSale.TotalAmount = sale.TotalAmount;
        existingSale.IsCancelled = sale.IsCancelled;

        await _salesRepository.UpdateAsync(existingSale);

        var result = _mapper.Map<SalesModel>(existingSale);

        await FulfillCustomerData(result);

        return result;
    }

    public async Task<string> DeleteSaleAsync(int id)
    {
        var sale = await _salesRepository.GetSaleByIdAsync(id);

        if (sale == null)
        {
            throw new BusinessException(new ErrorResponse()
            {
                Type = Errors.ResourceNotFoundType,
                Error = Errors.SaleNotFoundMessage,
                Detail = string.Format(Errors.SaleNotFoundDetail, id)
            });
        }

        sale.IsCancelled = true;
        sale.SalesDate = sale.SalesDate.ToUniversalTime();
        await _salesRepository.UpdateAsync(sale);

        return "Sale successfully deleted";
    }

    private Task<decimal> CalculateDiscountedPriceAsync(Item item)
    {
        return Task.Run(() =>
        {
            if (item.Quantity > MAX_QUANTITY)
            {
                throw new BusinessException(new ErrorResponse()
                {
                    Type = Errors.LessThanOrEqualValidatorType,
                    Error = string.Format(Errors.LessThanOrEqualValidatorMessage, $"Product : {item.ItemId}. Property : Quantity"),
                    Detail = Errors.LessThanOrEqualValidatorDetail
                });
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

            if (product is null)
            {
                throw new BusinessException(new ErrorResponse()
                {
                    Type = Errors.ResourceNotFoundType,
                    Error = Errors.ProductNotFoundMessage,
                    Detail = string.Format(Errors.ProductNotFoundDetail, item.Id)
                });
            }

            ProcessProduct(item, product, action);

            return product;
        });

        await Task.WhenAll(productTasks);
    }

    private async Task FulfillCustomerData(SalesModel sale)
    {
        var customer = await _customerApi.GetCustomerAsync(sale.Customer.Id);

        if(customer is null)
        {
            throw new BusinessException(new ErrorResponse()
            {
                Type = Errors.ResourceNotFoundType,
                Error = Errors.CustomerNotFoundMessage,
                Detail = string.Format(Errors.CustomerNotFoundDetail, sale.Customer.Id)
            });
        }

        ProcessCustomer(sale.Customer, customer);
    }
}
