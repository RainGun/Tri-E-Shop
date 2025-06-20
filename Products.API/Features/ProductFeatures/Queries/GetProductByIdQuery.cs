using MediatR;
using Products.API.Dtos;

namespace Products.API.Features.ProductFeatures.Queries;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public string Id { get; set; } = string.Empty;
}