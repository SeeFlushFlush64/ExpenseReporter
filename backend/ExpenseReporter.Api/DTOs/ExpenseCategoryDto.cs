namespace ExpenseReporter.Api.DTOs
{
    public class ExpenseCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
    }
}
