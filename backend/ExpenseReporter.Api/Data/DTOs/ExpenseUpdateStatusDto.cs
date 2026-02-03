using System.ComponentModel.DataAnnotations;

namespace ExpenseReporter.Api.Data.DTOs
{
    public class ExpenseUpdateStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Pending|Approved|Rejected)$",
        ErrorMessage = "Status must be 'Pending', 'Approved', or 'Rejected'")]
        public string Status { get; set; } = string.Empty;
    }
}
