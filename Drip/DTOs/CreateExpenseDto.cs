namespace Drip.DTOs;

// What the client sends when creating a new expense
public class CreateExpenseDto
{
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpenseDate { get; set; }   // optional — defaults to today
}

// "Data Transfer Object." It's a filter. You don't want clients to see or send every database column. DTOs control exactly what data enters and exits your API.

// Interview tip: This is why ORMs like EF Core exist — they reduce the number of places you need to change when the schema evolves. Dapper gives you speed and control, EF Core gives you safety and convenience. Tradeoff.

