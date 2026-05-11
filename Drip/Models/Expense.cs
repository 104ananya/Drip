namespace Drip.Models;

// Mirrors the "expenses" table in PostgreSQL — each property = one column
public class Expense
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public int? CategoryId { get; set; }          // (?) nullable — category is optional
    public string? PaymentMethod { get; set; }    // (?) nullable — optional
    public string? Description { get; set; }      // (?) nullable — optional
    public DateOnly ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Note: This model is used internally in the API to represent expenses as they exist in the database.
// The API does NOT expose this model directly to clients. Instead, it uses DTOs (Data Transfer Objects) to control what data is sent to and received from clients.
// In C#, a normal int can't be null — int? means "this integer is allowed to be empty/null."