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
        logger.LogInformation("Attempting to get all products from external API.");
        var client = httpClientFactory.CreateClient("PlatformSH");

        try
        {
            var response = await client.GetAsync("products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return Ok(products);
            }

            // --- IMPROVEMENT ---
            // If the call fails, log it and pass the external error message through.
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogWarning("The external API returned a non-success status code: {StatusCode}. Content: {ErrorContent}", response.StatusCode, errorContent);
            return StatusCode((int)response.StatusCode, errorContent);
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

            // If the call fails, log it and pass the external error message through.
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogWarning("The external API returned a non-success status code: {StatusCode} for product ID: {ProductId}. Content: {ErrorContent}", response.StatusCode, id, errorContent);
            return StatusCode((int)response.StatusCode, errorContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while fetching product with ID: {ProductId}", id);
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}