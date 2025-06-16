using System.ComponentModel.DataAnnotations;

namespace Orders.API.Dtos;
public class OrderCreateDto
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [MinLength(1)]
    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class OrderItemDto
{
    [Required]
    public string? ProductId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }
}

