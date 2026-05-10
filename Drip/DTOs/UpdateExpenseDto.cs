namespace Drip.DTOs;

// What the client sends when updating an expense (same fields as Create)
public class UpdateExpenseDto
{
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpenseDate { get; set; }
}
