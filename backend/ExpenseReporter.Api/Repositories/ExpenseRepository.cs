using ExpenseReporter.Api.Data;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseReporter.Api.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        // ─── Employee operations ─────────────────────────────────

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        // ─── ExpenseCategory operations ──────────────────────────

        public async Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync()
        {
            return await _context.ExpenseCategories.ToListAsync();
        }

        public async Task<ExpenseCategory?> GetCategoryByIdAsync(int id)
        {
            return await _context.ExpenseCategories.FindAsync(id);
        }

        public async Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategory category)
        {
            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // ─── Expense operations ──────────────────────────────────

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetExpensesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByStatusAsync(string status)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .Where(e => e.Status == status)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> UpdateExpenseStatusAsync(int id, string status)
        {
            var expense = await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new InvalidOperationException($"Expense with Id {id} not found.");

            expense.Status = status;
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
