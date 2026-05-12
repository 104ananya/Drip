using Drip.DTOs;
using Drip.Services;
using Microsoft.AspNetCore.Mvc;   // For ControllerBase, ApiController, HttpGet, etc.

namespace Drip.Controllers;

[ApiController]
[Route("api/[controller]")]    // → /api/expenses
public class ExpensesController : ControllerBase        // INHERITANCE
{
    private readonly IExpenseService _expenseService;

    // DI — IExpenseService is injected automatically
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    // Hardcoded userId for now — will come from JWT token in Phase 3
    private int GetUserId() => 1;

    // GET /api/expenses
    [HttpGet]
    public async Task<IActionResult> GetAll()       // Async wrapper
    {
        var expenses = await _expenseService.GetAllAsync(GetUserId());
        return Ok(expenses);
    }

    // GET /api/expenses/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await _expenseService.GetByIdAsync(id, GetUserId());
        if (expense == null)
            return NotFound(new { message = "Expense not found" });

        return Ok(expense);
    }

    // POST /api/expenses
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseDto dto)
    {
        var expense = await _expenseService.CreateAsync(dto, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    // PUT /api/expenses/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseDto dto)
    {
        var updated = await _expenseService.UpdateAsync(id, dto, GetUserId());
        if (!updated)
            return NotFound(new { message = "Expense not found" });

        return NoContent();   // 204 — success, no body needed
    }

    // DELETE /api/expenses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _expenseService.DeleteAsync(id, GetUserId());
        if (!deleted)
            return NotFound(new { message = "Expense not found" });

        return NoContent();   // 204
    }
}
