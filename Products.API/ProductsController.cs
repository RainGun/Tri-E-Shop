using Microsoft.AspNetCore.Mvc;
using Products.API.Dtos;
using System.Reflection.Metadata;

namespace Products.API.Controllers
{ 

    //Attributes are required for driver discovery and operation.Finalize Products API setup in Program.cs.
    
    [ApiController]
    [Route("api/[controller]")] 
    public class ProductsController : ControllerBase // Inherits from ControllerBase.
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IHttpClientFactory httpClientFactory, ILogger<ProductsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet] // The HttpGet defines the method as a get endpoint.
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("Attempting to get all products from external API.");
            var client = _httpClientFactory.CreateClient("PlatformSH");

            try
            {
                var response = await client.GetAsync("products");

                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                    return Ok(products);
                }

                _logger.LogWarning("The external API returned a non-success status code: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching all products.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
 
        //[HttpGet("{id}")] handles GET endpoint for retrieving a product by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            _logger.LogInformation("Attempting to get product with ID: {ProductId}", id);
            var client = _httpClientFactory.CreateClient("PlatformSH");

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

                _logger.LogWarning("The external API returned a non-success status code: {StatusCode} for product ID: {ProductId}", response.StatusCode, id);
                return StatusCode((int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching product with ID: {ProductId}", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}