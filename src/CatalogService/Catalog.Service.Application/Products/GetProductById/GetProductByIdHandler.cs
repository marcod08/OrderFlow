using Catalog.Service.Application.DTOs;
using Catalog.Service.Application.Interfaces;
using MediatR;

namespace Catalog.Service.Application.Products.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }
    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null) return null;

        return new ProductResponse(product.Id, product.Name, product.Price, product.StockQuantity);
    }
}