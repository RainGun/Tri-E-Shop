using MediatR;
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Queries;
// This Query carries the data needed to get an order (the Id).
public class GetOrderByIdQuery : IRequest<Order?>
{
    public Guid Id { get; set; }
}