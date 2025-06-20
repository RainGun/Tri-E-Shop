using MediatR;
using Products.API.Dtos;

namespace Products.API.Features.ProductFeatures.Queries;

public class GetAllProductsQueryHandler(IHttpClientFactory httpClientFactory, ILogger<GetAllProductsQueryHandler> logger)
    : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>?>
{
    public async Task<IEnumerable<ProductDto>?> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to get all products from external API.");
        var client = httpClientFactory.CreateClient("PlatformSH");

        // Note: We are not implementing the full error propagation here for brevity,
        // but the logic would be the same as we did in the controller before.
        var response = await client.GetAsync("products", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>(cancellationToken: cancellationToken);
            return products;
        }

        return null; // Or handle the error as we did before
    }
}