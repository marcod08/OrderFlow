using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Service.Application.DTOs;
using Ordering.Service.Application.Orders.CancelOrder;
using Ordering.Service.Application.Orders.CreateOrder;
using Ordering.Service.Application.Orders.GetOrderById;

namespace Ordering.Service.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? throw new UnauthorizedAccessException("User ID not found in token.");

        var command = new CreateOrderCommand(request.ProductId, request.Quantity, userId);
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateOrder), new { id = result }, result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? throw new UnauthorizedAccessException("User ID not found in token.");

        var result = await mediator.Send(new GetOrderByIdQuery(id, userId), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? throw new UnauthorizedAccessException("User ID not found in token.");

        var result = await mediator.Send(new CancelOrderCommand(id, userId), cancellationToken);
        if (!result) return NotFound();
        return NoContent();
    }
}