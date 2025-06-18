using Microsoft.AspNetCore.Mvc;
using Products.API.Dtos;

namespace Products.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IHttpClientFactory httpClientFactory, ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        // We use the parameter 'logger' directly
        logger.LogInformation("Attempting to get all products from external API.");
        // We use the parameter 'httpClientFactory' directly
        var client = httpClientFactory.CreateClient("PlatformSH");
        
        try
        {
            var response = await client.GetAsync("products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return Ok(products);
            }

            logger.LogWarning("The external API returned a non-success status code: {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while fetching all products.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        logger.LogInformation("Attempting to get product with ID: {ProductId}", id);
        var client = httpClientFactory.CreateClient("PlatformSH");

        try
        {
            var response = await client.GetAsync($"products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                return Ok(product);
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            logger.LogWarning("The external API returned a non-success status code: {StatusCode} for product ID: {ProductId}", response.StatusCode, id);
            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while fetching product with ID: {ProductId}", id);
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}