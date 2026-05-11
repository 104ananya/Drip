# Drip — Bug Log

> Every bug encountered, root cause, and fix. Great for interviews — shows debugging skills.

---

## Bug #1 — `Error parsing column 6 (expensedate - DateOnly)`

**When:** Day 3 — first POST /api/expenses attempt  
**Error:** `System.InvalidCastException: Object must implement IConvertible`  
**Endpoint:** POST /api/expenses

### What Happened

1. SQL `INSERT...RETURNING` ran successfully — row was inserted in PostgreSQL
2. PostgreSQL returned the row — `expense_date` column (type `DATE`)
3. Npgsql (PostgreSQL driver) maps `DATE` → `DateOnly` in .NET
4. Dapper tried to put `DateOnly` into our C# property `ExpenseDate` which was `DateTime`
5. Crash — can't convert `DateOnly` → `DateTime`

### Root Cause

Type mismatch between PostgreSQL and C#:

| PostgreSQL type | Npgsql returns | Our C# property was | Should be |
|---|---|---|---|
| `DATE` | `DateOnly` | `DateTime` | `DateOnly` |
| `TIMESTAMP` | `DateTime` | `DateTime` | `DateTime` ✅ |

### Fix

Changed `DateTime` → `DateOnly` for `ExpenseDate` in:
- `Models/Expense.cs`
- `DTOs/CreateExpenseDto.cs`
- `DTOs/UpdateExpenseDto.cs`
- `DTOs/ExpenseResponseDto.cs`

### Lesson

Always match C# types to the actual PostgreSQL column type. `DATE` ≠ `TIMESTAMP`.

---

## Bug #2 — `DateOnly cannot be used as a parameter value`

**When:** Day 3 — second POST attempt (after fixing Bug #1)  
**Error:** `System.NotSupportedException: The member ExpenseDate of type System.DateOnly cannot be used as a parameter value`  
**Endpoint:** POST /api/expenses

### What Happened

1. After fixing Bug #1, the C# property was now `DateOnly` ✅
2. But the SQL also has `@ExpenseDate` as a **parameter** being sent TO PostgreSQL
3. Dapper tried to send `DateOnly` as a SQL parameter
4. Dapper was written before `DateOnly` existed in .NET — it doesn't know how to handle it
5. Crash — "I don't know what to do with this type"

### Root Cause

Dapper lacks native `DateOnly` support. It was built before .NET 6 introduced `DateOnly`.

### Fix

Created `Data/DateOnlyTypeHandler.cs` — a custom type handler that teaches Dapper:
- **Sending to DB:** Convert `DateOnly` → `DateTime`
- **Reading from DB:** Accept `DateOnly` or convert `DateTime` → `DateOnly`

Registered in `Program.cs`:
```csharp
SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
```

### Lesson

When using Dapper with newer .NET types, you may need custom type handlers. This is a known Dapper limitation — the standard community fix.

---

## Bug #3 — `Unable to cast DateOnly to DateTime` (in our handler)

**When:** Day 3 — third POST attempt (after adding type handler)  
**Error:** `System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.DateTime'`  
**File:** `Data/DateOnlyTypeHandler.cs`, line 19

### What Happened

1. Type handler's `Parse` method did `DateOnly.FromDateTime((DateTime)value)`
2. But Npgsql was already returning `DateOnly` — not `DateTime`
3. Casting `DateOnly` as `DateTime` fails — they're different types

### Root Cause

Assumed Npgsql returns `DateTime` for `DATE` columns. It actually returns `DateOnly` directly.

### Fix

Check the type before converting:
```csharp
public override DateOnly Parse(object value)
{
    if (value is DateOnly dateOnly)
        return dateOnly;

    return DateOnly.FromDateTime((DateTime)value);
}
```

### Lesson

Don't assume what type a database driver returns. Check first, convert second. The `is` pattern match handles both cases safely.

---

## Side Effect — Duplicate Rows (3 identical expenses)

**Caused by:** Bugs #1, #2, #3

### What Happened

The SQL was `INSERT...RETURNING` — two steps:
1. `INSERT` — saves the row ✅ (this always succeeded)
2. `RETURNING` — reads it back ❌ (this is where Dapper crashed)

Each failed attempt still inserted a row. The error was only on the read-back step.

```
Attempt 1: INSERT ✅ → RETURNING → parse crash ❌ → Bug #1
Attempt 2: INSERT ✅ → RETURNING → parameter crash ❌ → Bug #2
Attempt 3: INSERT ✅ → RETURNING → parse crash ❌ → Bug #3
Attempt 4: INSERT ✅ → RETURNING ✅ → 201 Created
```

### Lesson

An error after INSERT doesn't mean the INSERT failed. In production, use **transactions** to roll back if anything fails — so you don't get ghost data.

### Cleanup

Deleted duplicate rows via DELETE /api/expenses/1 and DELETE /api/expenses/2.

---

*Updated as bugs are encountered...*
