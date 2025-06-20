using MediatR;
using Products.API.Dtos;

namespace Products.API.Features.ProductFeatures.Queries;

public class GetProductByIdQueryHandler(IHttpClientFactory httpClientFactory, ILogger<GetProductByIdQueryHandler> logger)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to get product with ID: {ProductId}", request.Id);
        var client = httpClientFactory.CreateClient("PlatformSH");

        var response = await client.GetAsync($"products/{request.Id}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);
            return product;
        }

        return null;
    }
}