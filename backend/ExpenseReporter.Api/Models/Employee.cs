namespace ExpenseReporter.Api.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    // Navigation property
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}