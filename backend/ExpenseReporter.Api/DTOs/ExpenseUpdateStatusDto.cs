namespace ExpenseReporter.Api.DTOs
{
    public class ExpenseUpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;   // "Pending", "Approved", "Rejected"
    }
}
