namespace Orders.API.Entities;
public class OrderDetail
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string? ProductId { get; set; } // The ID from the external API
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at the time of purchase
}