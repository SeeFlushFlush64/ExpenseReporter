namespace ExpenseReporter.Api.Data.DTOs
{
    public class MonthlyTrendDto
    {
        public List<MonthlyDataPointDto> DataPoints { get; set; } = new();
        public decimal OverallTotal { get; set; }
        public int TotalMonths { get; set; }
    }

    public class MonthlyDataPointDto
    {
        public string Month { get; set; } = string.Empty;
        public string MonthName { get; set; } = string.Empty;
        public Dictionary<string, decimal> CategorySpending { get; set; } = new();
        public decimal MonthTotal { get; set; }
        public int ExpenseCount { get; set; }
    }

    public class DepartmentComparisonDto
    {
        public List<DepartmentSpendingDto> Departments { get; set; } = new();
        public decimal OverallTotal { get; set; }
        public string TopDepartment { get; set; } = string.Empty;
    }

    public class DepartmentSpendingDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int EmployeeCount { get; set; }
        public int ExpenseCount { get; set; }
        public decimal AveragePerEmployee { get; set; }
        public double PercentageOfTotal { get; set; }
    }

    public class TopSpendersDto
    {
        public List<EmployeeSpendingSummaryDto> Employees { get; set; } = new();
        public decimal TotalTracked { get; set; }
    }
    
    public class EmployeeSpendingSummaryDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int ExpenseCount { get; set; }
        public decimal AverageExpenseAmount { get; set; }
        public DateTime? LastExpenseDate { get; set; }
    }

    public class BudgetStatusDto
    {
        public List<CategoryBudgetStatusDto> Categories { get; set; } = new();
        public decimal TotalBudgeted { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal TotalRemaining { get; set; }
        public int CategoriesOverBudget { get; set; }
    }

    public class CategoryBudgetStatusDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
        public decimal AmountSpent { get; set; }
        public decimal Remaining { get; set; }
        public double PercentageUsed { get; set; }
        public int ExpenseCount { get; set; }
        public bool IsOverBudget => AmountSpent > BudgetLimit;
        public string StatusColor => PercentageUsed switch
        {
            < 70 => "green",
            < 90 => "yellow",
            _ => "red"
        };
    }

    public class CategoryBreakdownDto
    {
        public List<CategorySpendingDetailDto> Categories { get; set; } = new();
        public decimal TotalSpent { get; set; }
    }

    public class CategorySpendingDetailDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal AmountSpent { get; set; }
        public int ExpenseCount { get; set; }
        public double PercentageOfTotal { get; set; }
        public decimal AverageExpenseAmount { get; set; }
    }

    public class SpendingSummaryDto
    {
        public decimal CurrentMonthTotal { get; set; }
        public int CurrentMonthExpenseCount { get; set; }
        public decimal AllTimeTotal { get; set; }
        public int TotalExpenseCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal LastMonthTotal { get; set; }
        public decimal MonthOverMonthChange { get; set; }
        public double MonthOverMonthPercentage { get; set; }
        public bool IsIncreasing => MonthOverMonthChange > 0;
    }
}
