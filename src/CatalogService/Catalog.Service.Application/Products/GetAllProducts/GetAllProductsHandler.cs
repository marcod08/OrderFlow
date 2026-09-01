using Catalog.Service.Application.DTOs;
using Catalog.Service.Application.Interfaces;
using MediatR;

namespace Catalog.Service.Application.Products.GetAllProducts;

public class GetAllProductsHandler(IProductRepository productRepository) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);

        return products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.StockQuantity));
    }
}