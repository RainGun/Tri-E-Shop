using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Dtos;
using Orders.API.Features.OrderFeatures.Commands;

namespace Orders.API.Controllers; // File-scoped namespace

[ApiController]
[Route("api/[controller]")]
// Primary constructor for dependency injection
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderRequest)
    {
        var command = new CreateOrderCommand
        {
            CustomerId = orderRequest.CustomerId,
            OrderItems = orderRequest.OrderItems
        };

        var createdOrder = await mediator.Send(command);

        // This GetOrderById still needs to be refactored to CQRS
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{id}")]
    public IActionResult GetOrderById(Guid id)
    {
        // This part could also be refactored into a Query and QueryHandler.
        // For now, we leave it as is to show the contrast.
        return Ok($"Endpoint to get order {id} is not fully implemented with CQRS yet.");
    }
}