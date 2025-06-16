namespace Payment.API.Dtos
{
    // DTO for the incoming payment request
    public class PaymentRequestDto
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? CardNumber { get; set; }
    }

    // DTO for the outgoing payment response
    public class PaymentResponseDto
    {
        public Guid PaymentId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}