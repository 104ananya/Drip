using Dapper;       // Without using Dapper;, the connection.QueryAsync() call would show a red error — C# wouldn't know that method exists.
using Drip.Data;
using Drip.DTOs;

namespace Drip.Services;

// Implementation — HOW the service does it (raw SQL via Dapper)
public class ExpenseService : IExpenseService
{
    private readonly DapperContext _context;

    // DI — DapperContext is injected automatically
    public ExpenseService(DapperContext context)
    {
        _context = context;
    }

    // GET all expenses for a user (with category name via JOIN)
    public async Task<IEnumerable<ExpenseResponseDto>> GetAllAsync(int userId)
    {
        var sql = @"SELECT e.id, e.amount, e.merchant, e.category_id AS CategoryId, 
                           c.name AS CategoryName, e.payment_method AS PaymentMethod, 
                           e.description, e.expense_date AS ExpenseDate, e.created_at AS CreatedAt
                    FROM expenses e
                    LEFT JOIN categories c ON e.category_id = c.id
                    WHERE e.user_id = @UserId
                    ORDER BY e.expense_date DESC";

        using var connection = _context.CreateConnection();             // opens DB connection, auto-closes when done
        return await connection.QueryAsync<ExpenseResponseDto>(sql, new { UserId = userId });
    }

    // GET single expense by ID
    public async Task<ExpenseResponseDto?> GetByIdAsync(int id, int userId)
    {
        var sql = @"SELECT e.id, e.amount, e.merchant, e.category_id AS CategoryId, 
                           c.name AS CategoryName, e.payment_method AS PaymentMethod, 
                           e.description, e.expense_date AS ExpenseDate, e.created_at AS CreatedAt
                    FROM expenses e
                    LEFT JOIN categories c ON e.category_id = c.id
                    WHERE e.id = @Id AND e.user_id = @UserId";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ExpenseResponseDto>(sql, new { Id = id, UserId = userId });
    }

    // CREATE a new expense
    public async Task<ExpenseResponseDto> CreateAsync(CreateExpenseDto dto, int userId)
    {
        var sql = @"INSERT INTO expenses (user_id, amount, merchant, category_id, payment_method, description, expense_date)
                    VALUES (@UserId, @Amount, @Merchant, @CategoryId, @PaymentMethod, @Description, @ExpenseDate)
                    RETURNING id, amount, merchant, category_id AS CategoryId, payment_method AS PaymentMethod, 
                              description, expense_date AS ExpenseDate, created_at AS CreatedAt";

        using var connection = _context.CreateConnection();
        var expense = await connection.QuerySingleAsync<ExpenseResponseDto>(sql, new
        {
            UserId = userId,
            dto.Amount,
            dto.Merchant,
            dto.CategoryId,
            dto.PaymentMethod,
            dto.Description,
            ExpenseDate = dto.ExpenseDate ?? DateTime.Today
        });

        return expense;
    }

    // UPDATE an existing expense
    public async Task<bool> UpdateAsync(int id, UpdateExpenseDto dto, int userId)
    {
        var sql = @"UPDATE expenses 
                    SET amount = @Amount, merchant = @Merchant, category_id = @CategoryId,
                        payment_method = @PaymentMethod, description = @Description, 
                        expense_date = @ExpenseDate, updated_at = NOW()
                    WHERE id = @Id AND user_id = @UserId";

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            Id = id,
            UserId = userId,
            dto.Amount,
            dto.Merchant,
            dto.CategoryId,
            dto.PaymentMethod,
            dto.Description,
            ExpenseDate = dto.ExpenseDate ?? DateTime.Today
        });

        return rowsAffected > 0; // true if expense was found and updated
    }

    // DELETE an expense
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var sql = "DELETE FROM expenses WHERE id = @Id AND user_id = @UserId";

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });

        return rowsAffected > 0; // true if expense was found and deleted
    }
}
