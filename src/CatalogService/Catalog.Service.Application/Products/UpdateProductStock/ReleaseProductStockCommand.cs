using MediatR;

namespace Catalog.Service.Application.Products.UpdateProductStock;

public record ReleaseProductStockCommand(Guid ProductId, int Quantity) : IRequest<bool>;