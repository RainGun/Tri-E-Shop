using MediatR;
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Commands; // File-scoped namespace

// Primary constructor for dependency injection
public class CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, Order>
{
    private static readonly List<Order> _orders = new();
    public Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var newOrderDetails = command.OrderItems.Select(item => new OrderDetail
        {
            Id = Guid.NewGuid(),
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = 19.99m // Mock price
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
        logger.LogInformation("Successfully created order {OrderId}", newOrder.Id);

        return Task.FromResult(newOrder);
    }
}