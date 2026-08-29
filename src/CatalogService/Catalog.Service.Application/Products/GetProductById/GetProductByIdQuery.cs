using Catalog.Service.Application.DTOs;
using MediatR;

namespace Catalog.Service.Application.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;