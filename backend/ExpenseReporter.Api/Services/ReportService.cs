using ExpenseReporter.Api.Data.DTOs;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Models;

namespace ExpenseReporter.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IExpenseRepository _repository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IExpenseRepository repository, ILogger<ReportService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // ─── Employee operations ─────────────────────────────────

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            _logger.LogInformation("Fetching all employees");
            var employees = await _repository.GetAllEmployeesAsync();
            return employees.Select(MapToEmployeeDto);
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation("Fetching employee with Id: {EmployeeId}", id);
            var employee = await _repository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with Id {EmployeeId} not found", id);
                return null;
            }

            return MapToEmployeeDto(employee);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
            _logger.LogInformation("Creating new employee: {Email}", dto.Email);

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                HireDate = dto.HireDate
            };

            var created = await _repository.CreateEmployeeAsync(employee);
            _logger.LogInformation("Employee created successfully with Id: {EmployeeId}", created.Id);

            return MapToEmployeeDto(created);
        }

        // ─── Category operations ─────────────────────────────────

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Fetching all expense categories");
            var categories = await _repository.GetAllCategoriesAsync();
            return categories.Select(MapToCategoryDto);
        }

        public async Task<ExpenseCategoryDto?> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Fetching expense category with Id: {CategoryId}", id);
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Expense category with Id {CategoryId} not found", id);
                return null;
            }

            return MapToCategoryDto(category);
        }

        public async Task<ExpenseCategoryDto> CreateCategoryAsync(ExpenseCategoryCreateDto dto)
        {
            _logger.LogInformation("Creating new expense category: {CategoryName}", dto.Name);

            var category = new ExpenseCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                BudgetLimit = dto.BudgetLimit
            };

            var created = await _repository.CreateCategoryAsync(category);
            _logger.LogInformation("Expense category created successfully with Id: {CategoryId}", created.Id);

            return MapToCategoryDto(created);
        }

        // ─── Expense operations ──────────────────────────────────

        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            _logger.LogInformation("Fetching all expenses");
            var expenses = await _repository.GetAllExpensesAsync();
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<ExpenseDto?> GetExpenseByIdAsync(int id)
        {
            _logger.LogInformation("Fetching expense with Id: {ExpenseId}", id);
            var expense = await _repository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                _logger.LogWarning("Expense with Id {ExpenseId} not found", id);
                return null;
            }

            return MapToExpenseDto(expense);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeIdAsync(int employeeId)
        {
            _logger.LogInformation("Fetching expenses for employee Id: {EmployeeId}", employeeId);
            var expenses = await _repository.GetExpensesByEmployeeIdAsync(employeeId);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            _logger.LogInformation("Fetching expenses for category Id: {CategoryId}", categoryId);
            var expenses = await _repository.GetExpensesByCategoryIdAsync(categoryId);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByStatusAsync(string status)
        {
            _logger.LogInformation("Fetching expenses with status: {Status}", status);
            var expenses = await _repository.GetExpensesByStatusAsync(status);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Fetching expenses between {StartDate} and {EndDate}", startDate, endDate);
            var expenses = await _repository.GetExpensesByDateRangeAsync(startDate, endDate);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto dto)
        {
            _logger.LogInformation("Creating new expense for employee Id: {EmployeeId}, Amount: {Amount}",
                dto.EmployeeId, dto.Amount);

            var expense = new Expense
            {
                EmployeeId = dto.EmployeeId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate,
                Description = dto.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateExpenseAsync(expense);

            // Re-fetch to get navigation properties populated
            var full = await _repository.GetExpenseByIdAsync(created.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created expense.");

            _logger.LogInformation("Expense created successfully with Id: {ExpenseId}", full.Id);

            return MapToExpenseDto(full);
        }

        public async Task<ExpenseDto> UpdateExpenseStatusAsync(int id, ExpenseUpdateStatusDto dto)
        {
            _logger.LogInformation("Updating expense Id: {ExpenseId} status to: {Status}", id, dto.Status);

            var updated = await _repository.UpdateExpenseStatusAsync(id, dto.Status);

            _logger.LogInformation("Expense Id: {ExpenseId} status updated successfully", id);

            return MapToExpenseDto(updated);
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            _logger.LogInformation("Deleting expense with Id: {ExpenseId}", id);

            var deleted = await _repository.DeleteExpenseAsync(id);

            if (deleted)
            {
                _logger.LogInformation("Expense Id: {ExpenseId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete expense Id: {ExpenseId} - not found", id);
            }

            return deleted;
        }

        // ─── Summary / reporting ─────────────────────────────────

        public async Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId)
        {
            _logger.LogInformation("Calculating total expenses for employee Id: {EmployeeId}", employeeId);
            var expenses = await _repository.GetExpensesByEmployeeIdAsync(employeeId);
            var total = expenses.Sum(e => e.Amount);
            _logger.LogInformation("Total expenses for employee Id {EmployeeId}: {Total}", employeeId, total);
            return total;
        }

        public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Calculating total expenses for category Id: {CategoryId}", categoryId);
            var expenses = await _repository.GetExpensesByCategoryIdAsync(categoryId);
            var total = expenses.Sum(e => e.Amount);
            _logger.LogInformation("Total expenses for category Id {CategoryId}: {Total}", categoryId, total);
            return total;
        }

        public async Task<Dictionary<string, decimal>> GetExpenseSummaryByCategoryAsync()
        {
            _logger.LogInformation("Generating expense summary by category");
            var expenses = await _repository.GetAllExpensesAsync();
            var summary = expenses
                .GroupBy(e => e.Category?.Name ?? "Uncategorized")
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.Amount)
                );
            _logger.LogInformation("Expense summary generated for {Count} categories", summary.Count);
            return summary;
        }

        public async Task<Dictionary<string, long>> GetExpenseCountByStatusAsync()
        {
            _logger.LogInformation("Generating expense count by status");
            var expenses = await _repository.GetAllExpensesAsync();
            var counts = expenses
                .GroupBy(e => e.Status)
                .ToDictionary(
                    g => g.Key,
                    g => (long)g.Count()
                );
            _logger.LogInformation("Expense count generated for {Count} statuses", counts.Count);
            return counts;
        }

        // ─── Mapping helpers ─────────────────────────────────────

        private static EmployeeDto MapToEmployeeDto(Employee emp) => new()
        {
            Id = emp.Id,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email = emp.Email,
            Department = emp.Department,
            HireDate = emp.HireDate
        };

        private static ExpenseCategoryDto MapToCategoryDto(ExpenseCategory cat) => new()
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            BudgetLimit = cat.BudgetLimit
        };

        private static ExpenseDto MapToExpenseDto(Expense exp) => new()
        {
            Id = exp.Id,
            EmployeeId = exp.EmployeeId,
            EmployeeName = exp.Employee != null
                ? $"{exp.Employee.FirstName} {exp.Employee.LastName}"
                : string.Empty,
            CategoryId = exp.CategoryId,
            CategoryName = exp.Category?.Name ?? string.Empty,
            Amount = exp.Amount,
            ExpenseDate = exp.ExpenseDate,
            Description = exp.Description,
            Status = exp.Status,
            CreatedAt = exp.CreatedAt
        };
    }
}
