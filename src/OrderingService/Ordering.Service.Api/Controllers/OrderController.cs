using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Service.Application.Orders.CreateOrder;
using Ordering.Service.Application.Orders.GetOrderById;

namespace Ordering.Service.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateOrder), new { id = result }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }
}