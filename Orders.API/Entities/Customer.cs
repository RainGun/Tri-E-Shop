namespace Orders.API.Entities;
public class Customer
{   public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ShippingAddress { get; set; }
}

