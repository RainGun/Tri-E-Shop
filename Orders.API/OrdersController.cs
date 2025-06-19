using MediatR;
using Microsoft.AspNetCore.Authorization; // Make sure this using is present
using Microsoft.AspNetCore.Mvc;
using Orders.API.Dtos;
using Orders.API.Features.OrderFeatures.Commands;
using Orders.API.Features.OrderFeatures.Queries; // Make sure this using is present

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protect all endpoints in this controller by default
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
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var order = await mediator.Send(query);
        return order is not null ? Ok(order) : NotFound();
    }

    // This new endpoint is protected by a Role-based policy
    [HttpGet("admin/all-orders")]
    [Authorize(Roles = "Admin")] // This requires the user to have the "Admin" role in their token
    public IActionResult GetAllOrdersForAdmin()
    {
        // For this test, we just return a success message.
        // In a real app, you would return all orders from the InMemoryDataStore.
        return Ok("Successfully accessed an Admin-only endpoint.");
    }
}