using ExpenseReporter.Api.Models;

namespace ExpenseReporter.Api.Interfaces
{
    public interface IExpenseRepository
    {
        // ─── Employee operations ─────────────────────────────────

        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);

        // ─── ExpenseCategory operations ──────────────────────────

        Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync();
        Task<ExpenseCategory?> GetCategoryByIdAsync(int id);
        Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategory category);

        // ─── Expense operations ──────────────────────────────────

        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<IEnumerable<Expense>> GetExpensesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Expense>> GetExpensesByStatusAsync(string status);
        Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<Expense> UpdateExpenseStatusAsync(int id, string status);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
