using MediatR;
using Orders.API.Data; // Using our new in-memory store
using Orders.API.Entities;

namespace Orders.API.Features.OrderFeatures.Queries;

// This handler contains the logic to process the GetOrderByIdQuery.
public class GetOrderByIdQueryHandler(ILogger<GetOrderByIdQueryHandler> logger) : IRequestHandler<GetOrderByIdQuery, Order?>
{
    // The Handle method receives the query and returns the result.
    public Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Searching for order with ID {OrderId}", request.Id);

        var order = InMemoryDataStore.Orders.FirstOrDefault(o => o.Id == request.Id);

        // This is the improvement: log a warning if the order is not found.
        if (order is null)
        {
            logger.LogWarning("Order with ID {OrderId} not found.", request.Id);
        }

        return Task.FromResult(order);
    }
}