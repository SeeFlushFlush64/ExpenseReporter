namespace ExpenseReporter.Api.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;   // flattened from nav property
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;   // flattened from nav property
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
