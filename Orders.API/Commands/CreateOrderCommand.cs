using MediatR;
using Orders.API.Dtos;
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Commands
{
    // This command carries the data needed to create an order.
    // It implements IRequest<Order>, meaning it will return the created Order object.
    public class CreateOrderCommand : IRequest<Order>
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = [];
    }
}