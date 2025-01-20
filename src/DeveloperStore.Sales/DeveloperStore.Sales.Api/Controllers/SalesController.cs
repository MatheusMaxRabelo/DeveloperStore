using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Queries.Sales.GetSaleById;
using DeveloperStore.Sales.Application.Queries.Sales.GetSales;
using DeveloperStore.Sales.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get a paged list of sales based on a filter.
    /// </summary>
    /// <param name="filters"></param>
    /// <returns>A list of Sales</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<List<SalesDto>>))]
    public async Task<IActionResult> GetMembersAsync([FromQuery] IDictionary<string, string>? filters)
    {
        var request = new GetSalesQuery();
        if (filters != null)
        {
            request.Filters = filters.ToDictionary();
        }

       var result = await mediator.Send(request);

        return Ok(result);
    }

    /// <summary>
    /// Get a specificf sale based on id.
    /// </summary>
    /// <param name="filters"></param>
    /// <returns>A list of Sales</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<List<SalesDto>>))]
    public async Task<IActionResult> GetMembersAsync(int id)
    {
        var request = new GetSaleByIdQuery(id);

        var result = await mediator.Send(request);

        return Ok(result);
    }

    /// <summary>
    /// Create a new sale
    /// </summary>
    /// <param name="request"></param>
    /// <returns>New sale data</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SalesDto))]
    public async Task<IActionResult> AddMemberAsync([FromBody] CreateSaleCommand request)
    {
        var result = await mediator.Send(request);

        return Ok(result);
    }
}
