using Drip.DTOs;

namespace Drip.Services;

// Interface — defines WHAT the service can do (contract)
public interface IExpenseService
{
    Task<IEnumerable<ExpenseResponseDto>> GetAllAsync(int userId);
    Task<ExpenseResponseDto?> GetByIdAsync(int id, int userId);
    Task<ExpenseResponseDto> CreateAsync(CreateExpenseDto dto, int userId);
    Task<bool> UpdateAsync(int id, UpdateExpenseDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}

// The interface says "this service can GetAll, GetById, Create, Update, Delete" but doesn't contain the actual code.
// In Phase 2, when we switch to EF Core, we write a new implementation but the Controller code stays the same because it only depends on the interface, not the implementation. This is called "programming to an interface" and is a key principle of good software design.