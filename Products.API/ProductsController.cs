using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.API.Features.ProductFeatures.Queries;

namespace Products.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await mediator.Send(new GetAllProductsQuery());
        return products is not null ? Ok(products) : StatusCode(503, "External service is unavailable.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await mediator.Send(new GetProductByIdQuery { Id = id });
        return product is not null ? Ok(product) : NotFound();
    }
}