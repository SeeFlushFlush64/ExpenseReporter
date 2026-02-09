using ExpenseReporter.Api.Data.DTOs;

namespace ExpenseReporter.Api.Interfaces
{
    public interface IReportService
    {
        // ═══════════════════════════════════════════════════════════════
        // Existing Employee operations
        // ═══════════════════════════════════════════════════════════════

        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto);

        // ═══════════════════════════════════════════════════════════════
        // Existing Category operations
        // ═══════════════════════════════════════════════════════════════

        Task<IEnumerable<ExpenseCategoryDto>> GetAllCategoriesAsync();
        Task<ExpenseCategoryDto?> GetCategoryByIdAsync(int id);
        Task<ExpenseCategoryDto> CreateCategoryAsync(ExpenseCategoryCreateDto dto);

        // ═══════════════════════════════════════════════════════════════
        // Existing Expense operations
        // ═══════════════════════════════════════════════════════════════

        Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync();
        Task<ExpenseDto?> GetExpenseByIdAsync(int id);
        Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByStatusAsync(string status);
        Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto dto);
        Task<ExpenseDto> UpdateExpenseStatusAsync(int id, ExpenseUpdateStatusDto dto);
        Task<bool> DeleteExpenseAsync(int id);

        // ═══════════════════════════════════════════════════════════════
        // Existing Summary / reporting
        // ═══════════════════════════════════════════════════════════════

        Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId);
        Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId);
        Task<Dictionary<string, decimal>> GetExpenseSummaryByCategoryAsync();
        Task<Dictionary<string, long>> GetExpenseCountByStatusAsync();

        // ═══════════════════════════════════════════════════════════════
        // NEW: Analytics operations
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Get overall spending summary with current month stats and comparisons
        /// </summary>
        Task<SpendingSummaryDto> GetSpendingSummaryAsync();

        /// <summary>
        /// Get monthly spending trends for the specified number of months
        /// </summary>
        Task<MonthlyTrendDto> GetMonthlyTrendsAsync(int months);

        /// <summary>
        /// Get spending comparison across all departments
        /// </summary>
        Task<DepartmentComparisonDto> GetDepartmentComparisonAsync();

        /// <summary>
        /// Get top N employees by total spending
        /// </summary>
        Task<TopSpendersDto> GetTopSpendersAsync(int limit);

        /// <summary>
        /// Get budget status for all categories (budget vs actual)
        /// </summary>
        Task<BudgetStatusDto> GetBudgetStatusAsync();

        /// <summary>
        /// Get spending breakdown by category for pie/donut charts
        /// </summary>
        Task<CategoryBreakdownDto> GetCategoryBreakdownAsync();
    }
}
