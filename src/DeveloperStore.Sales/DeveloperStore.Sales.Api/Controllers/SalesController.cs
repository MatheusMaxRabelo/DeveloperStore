using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.DTOs;
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
    /// Get a paged list of members based on a filter.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A list of Members</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<List<SalesDto>>))]
    public async Task<IActionResult> GetMembersAsync([FromQuery] IDictionary<string, string>? filters)
    {
        var request = new GetSalesQuery();
        if (filters != null)
        {
            request.Filters = filters.ToDictionary();
        }

        await mediator.Send(request);

        return Ok();
    }

    /// <summary>
    /// Create a new member
    /// </summary>
    /// <param name="request"></param>
    /// <returns>New member's Id</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SalesDto))]
    public async Task<IActionResult> AddMemberAsync([FromBody] CreateSaleCommand request)
    {
        var result = await mediator.Send(request);

        return Ok(result);
    }
}
