using ExpenseReporter.Api.Data.DTOs;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Models;

namespace ExpenseReporter.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IExpenseRepository _repository;

        public ReportService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        // ─── Employee operations ─────────────────────────────────

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllEmployeesAsync();
            return employees.Select(MapToEmployeeDto);
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _repository.GetEmployeeByIdAsync(id);
            return employee != null ? MapToEmployeeDto(employee) : null;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                HireDate = dto.HireDate
            };

            var created = await _repository.CreateEmployeeAsync(employee);
            return MapToEmployeeDto(created);
        }

        // ─── Category operations ─────────────────────────────────

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllCategoriesAsync();
            return categories.Select(MapToCategoryDto);
        }

        public async Task<ExpenseCategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);
            return category != null ? MapToCategoryDto(category) : null;
        }

        public async Task<ExpenseCategoryDto> CreateCategoryAsync(ExpenseCategoryCreateDto dto)
        {
            var category = new ExpenseCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                BudgetLimit = dto.BudgetLimit
            };

            var created = await _repository.CreateCategoryAsync(category);
            return MapToCategoryDto(created);
        }

        // ─── Expense operations ──────────────────────────────────

        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            var expenses = await _repository.GetAllExpensesAsync();
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<ExpenseDto?> GetExpenseByIdAsync(int id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);
            return expense != null ? MapToExpenseDto(expense) : null;
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeIdAsync(int employeeId)
        {
            var expenses = await _repository.GetExpensesByEmployeeIdAsync(employeeId);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            var expenses = await _repository.GetExpensesByCategoryIdAsync(categoryId);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByStatusAsync(string status)
        {
            var expenses = await _repository.GetExpensesByStatusAsync(status);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await _repository.GetExpensesByDateRangeAsync(startDate, endDate);
            return expenses.Select(MapToExpenseDto);
        }

        public async Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto dto)
        {
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

            return MapToExpenseDto(full);
        }

        public async Task<ExpenseDto> UpdateExpenseStatusAsync(int id, ExpenseUpdateStatusDto dto)
        {
            var updated = await _repository.UpdateExpenseStatusAsync(id, dto.Status);
            return MapToExpenseDto(updated);
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            return await _repository.DeleteExpenseAsync(id);
        }

        // ─── Summary / reporting ─────────────────────────────────

        public async Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId)
        {
            var expenses = await _repository.GetExpensesByEmployeeIdAsync(employeeId);
            return expenses.Sum(e => e.Amount);
        }

        public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId)
        {
            var expenses = await _repository.GetExpensesByCategoryIdAsync(categoryId);
            return expenses.Sum(e => e.Amount);
        }

        public async Task<Dictionary<string, decimal>> GetExpenseSummaryByCategoryAsync()
        {
            var expenses = await _repository.GetAllExpensesAsync();
            return expenses
                .GroupBy(e => e.Category?.Name ?? "Uncategorized")
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.Amount)
                );
        }

        public async Task<Dictionary<string, long>> GetExpenseCountByStatusAsync()
        {
            var expenses = await _repository.GetAllExpensesAsync();
            return expenses
                .GroupBy(e => e.Status)
                .ToDictionary(
                    g => g.Key,
                    g => (long)g.Count()
                );
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
