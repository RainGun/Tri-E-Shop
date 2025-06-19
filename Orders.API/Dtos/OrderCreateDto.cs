using System.ComponentModel.DataAnnotations;

namespace Orders.API.Dtos;

public class OrderCreateDto
{
    [Required(ErrorMessage = "Customer ID is required")]
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "Order must have at least one item")]
    [MinLength(1, ErrorMessage = "Order must have at least one item")]
    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class OrderItemDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public string? ProductId { get; set; }

    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
    public int Quantity { get; set; }
}