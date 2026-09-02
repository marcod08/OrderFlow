using Catalog.Service.Application.Interfaces;
using MediatR;

namespace Catalog.Service.Application.Products.UpdateProductStock;

public class ReleaseProductStockHandler (IProductRepository repository) : IRequestHandler<ReleaseProductStockCommand, bool>
{
    public async Task<bool> Handle(ReleaseProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
        product.ReleaseStock(request.Quantity);

        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}