using ExpenseReporter.Api.Data.DTOs;

namespace ExpenseReporter.Api.Interfaces
{
    public interface IReportService
    {
        // ─── Employee queries ────────────────────────────────────

        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto);

        // ─── Category queries ────────────────────────────────────

        Task<IEnumerable<ExpenseCategoryDto>> GetAllCategoriesAsync();
        Task<ExpenseCategoryDto?> GetCategoryByIdAsync(int id);
        Task<ExpenseCategoryDto> CreateCategoryAsync(ExpenseCategoryCreateDto dto);

        // ─── Expense queries ─────────────────────────────────────

        Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync();
        Task<ExpenseDto?> GetExpenseByIdAsync(int id);
        Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByStatusAsync(string status);
        Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto dto);
        Task<ExpenseDto> UpdateExpenseStatusAsync(int id, ExpenseUpdateStatusDto dto);
        Task<bool> DeleteExpenseAsync(int id);

        // ─── Summary / reporting ─────────────────────────────────

        Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId);
        Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId);
        Task<Dictionary<string, decimal>> GetExpenseSummaryByCategoryAsync();
        Task<Dictionary<string, long>> GetExpenseCountByStatusAsync();
    }
}
