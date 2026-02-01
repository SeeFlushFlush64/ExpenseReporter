namespace ExpenseReporter.Api.Models;

public class ExpenseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BudgetLimit { get; set; }

    // Navigation property
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}