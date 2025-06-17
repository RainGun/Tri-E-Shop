using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Dtos;
using Orders.API.Features.OrderFeatures.Commands;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderRequest)
        {
            var command = new CreateOrderCommand
            {
                CustomerId = orderRequest.CustomerId,
                OrderItems = orderRequest.OrderItems
            };

            var createdOrder = await _mediator.Send(command);

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
}