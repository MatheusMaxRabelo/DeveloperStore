using DeveloperStore.Sales.Application.Models;
using Refit;

namespace DeveloperStore.Sales.Application.Services.Refit;

[Headers("accept: application/json")]
public interface IProductApi
{
    [Get("/products/{id}")]
    Task<Product> GetProductAsync(int id);
}
