using System.ComponentModel.DataAnnotations;

namespace ExpenseReporter.Api.Data.DTOs
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Department must be between 1 and 50 characters")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hire date is required")]
        public DateTime HireDate { get; set; }
    }
}
