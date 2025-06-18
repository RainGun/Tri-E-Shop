using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Dtos;
using Orders.API.Features.OrderFeatures.Commands;
using Orders.API.Features.OrderFeatures.Queries;

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
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        // Create the query object with the id from the route.
        var query = new GetOrderByIdQuery { Id = id };

        // Send the query to MediatR. MediatR will find the correct handler and execute it.
        var order = await mediator.Send(query);

        // Check the result from the handler. If it's null, return 404 Not Found.
        // Otherwise, return 200 OK with the order.
        return order is not null ? Ok(order) : NotFound();
    }
}