using System.ComponentModel.DataAnnotations;

namespace ExpenseReporter.Api.Data.DTOs
{
    public class ExpenseCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description must not exceed 200 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Budget limit is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Budget limit must be greater than 0")]
        public decimal BudgetLimit { get; set; }
    }
}
