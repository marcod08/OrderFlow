using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Service.Application.Orders;

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
}