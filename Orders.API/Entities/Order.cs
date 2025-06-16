namespace Orders.API.Entities;
public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = [];
}

