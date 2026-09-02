using Catalog.Service.Application.DTOs;
using Catalog.Service.Application.Products.CreateProduct;
using Catalog.Service.Application.Products.DeleteProduct;
using Catalog.Service.Application.Products.GetAllProducts;
using Catalog.Service.Application.Products.GetProductById;
using Catalog.Service.Application.Products.UpdateProductStock;
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reserve-stock")]
    public async Task<IActionResult> ReserveProductStock(Guid id, [FromBody] ReserveStockRequest request, CancellationToken cancellationToken)
    {
        var command = new ReserveProductStockCommand(id, request.Quantity);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/release-stock")]
    public async Task<IActionResult> ReleaseProductStock(Guid id, [FromBody] ReleaseStockRequest request, CancellationToken cancellationToken)
    {
        var command = new ReleaseProductStockCommand(id, request.Quantity);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}