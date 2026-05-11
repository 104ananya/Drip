namespace Drip.DTOs;

// What the API returns to the client — hides internal fields like UserId
public class ExpenseResponseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }     // from JOIN — friendly name instead of just ID
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
