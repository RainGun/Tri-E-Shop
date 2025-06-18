using Orders.API.Entities;

namespace Orders.API.Data;
public static class InMemoryDataStore
{
    // Our "database" of orders, now accessible from anywhere in the project.
    public static readonly List<Order> Orders = new();
}
