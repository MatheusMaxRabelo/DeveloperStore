using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(IMediator mediator) : ControllerBase
{
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
