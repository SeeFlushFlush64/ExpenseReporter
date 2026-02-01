namespace ExpenseReporter.Api.Data.DTOs
{
    public class ExpenseCategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
    }
}
