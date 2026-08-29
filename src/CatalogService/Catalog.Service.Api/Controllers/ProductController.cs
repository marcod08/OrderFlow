using Catalog.Service.Application.Products.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Service.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateProduct), new { id = result}, result);
    }
}