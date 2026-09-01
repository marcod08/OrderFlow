using Catalog.Service.Application.DTOs;
using MediatR;

namespace Catalog.Service.Application.Products.GetAllProducts;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductResponse>>;
