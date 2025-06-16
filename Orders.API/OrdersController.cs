using Microsoft.AspNetCore.Mvc;
using Orders.API.Dtos;
using Orders.API.Entities;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        // a simple in-memory list to store orders.       
        private static readonly List<Order> _orders = new();
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        // This endpoint handles the creation of a new order.
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateDto orderRequest)
        {
            // Basic validation
            if (orderRequest == null || !orderRequest.OrderItems.Any())
            {
                return BadRequest("Order request is invalid or has no items.");
            }

            _logger.LogInformation("Creating a new order for customer {CustomerId}", orderRequest.CustomerId);

           
            // from the Products.API to validate them.
            var newOrderDetails = orderRequest.OrderItems.Select(item => new OrderDetail
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = 19.99m
            }).ToList();

            decimal totalAmount = newOrderDetails.Sum(item => item.Quantity * item.UnitPrice);

            // Create the main Order entity.
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderRequest.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderDetails = newOrderDetails
            };

            _orders.Add(newOrder);
            _logger.LogInformation("Successfully created order {OrderId}", newOrder.Id);

            // Returning 'CreatedAtAction'
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        // This endpoint allows retrieving a specific order by its ID.
        [HttpGet("{id}")]
        public IActionResult GetOrderById(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}