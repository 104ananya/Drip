namespace Drip.Models;

// Mirrors the "categories" table in PostgreSQL
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property — one category has many expenses
    public List<Expense> Expenses { get; set; } = new();
}
