using DeveloperStore.Sales.Application.Models;
using Refit;

namespace DeveloperStore.Sales.Application.Services.Refit;


[Headers("accept: application/json")]
public interface ICustomerApi
{
    [Get("/users/{id}")]
    Task<Customer> GetCustomerAsync(int id);
}
