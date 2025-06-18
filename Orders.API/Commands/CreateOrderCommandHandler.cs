using MediatR;
using Orders.API.Data;
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Commands;
public class CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, Order>
{
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

        InMemoryDataStore.Orders.Add(newOrder);
        logger.LogInformation("Successfully created order {OrderId}", newOrder.Id);

        return Task.FromResult(newOrder);
    }
}