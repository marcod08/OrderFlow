using MediatR;

namespace Catalog.Service.Application.Products.CreateProduct;

public record CreateProductCommand(string Name, decimal Price, int InitialStock) : IRequest<Guid>;