using MediatR;
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        // This is our temporary in-memory "database"
        private static readonly List<Order> _orders = new();
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger)
        {
            _logger = logger;
        }

        // The Handle method contains the business logic.
        public Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var newOrderDetails = command.OrderItems.Select(item => new OrderDetail
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = 19.99m
            }).ToList();

            decimal totalAmount = newOrderDetails.Sum(item => item.Quantity * item.UnitPrice);

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderDetails = newOrderDetails
            };

            _orders.Add(newOrder);
            _logger.LogInformation("Successfully created order {OrderId}", newOrder.Id);

            // Return the newly created order.
            return Task.FromResult(newOrder);
        }
    }
}