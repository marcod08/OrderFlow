using MediatR;

namespace Catalog.Service.Application.Products.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest<bool>;