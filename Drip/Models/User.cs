namespace Drip.Models;

// Mirrors the "users" table in PostgreSQL
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property — one user has many expenses
    public List<Expense> Expenses { get; set; } = new();
}
