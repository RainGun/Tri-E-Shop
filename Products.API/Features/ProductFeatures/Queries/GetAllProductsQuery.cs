using MediatR;
using Products.API.Dtos;

namespace Products.API.Features.ProductFeatures.Queries;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>?>
{
}