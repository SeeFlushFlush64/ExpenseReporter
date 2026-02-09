using ExpenseReporter.Api.Data.DTOs;
using ExpenseReporter.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReporter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IReportService reportService, ILogger<DashboardController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/summary
        // Overall spending summary with current month stats
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("summary")]
        public async Task<ActionResult<SpendingSummaryDto>> GetSpendingSummary()
        {
            _logger.LogInformation("Fetching spending summary");

            var summary = await _reportService.GetSpendingSummaryAsync();
            return Ok(summary);
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/trends?months=6
        // Monthly spending trends across categories
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("trends")]
        public async Task<ActionResult<MonthlyTrendDto>> GetMonthlyTrends([FromQuery] int months = 6)
        {
            _logger.LogInformation("Fetching monthly trends for {Months} months", months);

            if (months < 1 || months > 24)
            {
                return BadRequest("Months parameter must be between 1 and 24");
            }

            var trends = await _reportService.GetMonthlyTrendsAsync(months);
            return Ok(trends);
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/department-comparison
        // Compare spending across departments
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("department-comparison")]
        public async Task<ActionResult<DepartmentComparisonDto>> GetDepartmentComparison()
        {
            _logger.LogInformation("Fetching department comparison");

            var comparison = await _reportService.GetDepartmentComparisonAsync();
            return Ok(comparison);
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/top-spenders?limit=5
        // Top employees by total spending
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("top-spenders")]
        public async Task<ActionResult<TopSpendersDto>> GetTopSpenders([FromQuery] int limit = 5)
        {
            _logger.LogInformation("Fetching top {Limit} spenders", limit);

            if (limit < 1 || limit > 50)
            {
                return BadRequest("Limit parameter must be between 1 and 50");
            }

            var topSpenders = await _reportService.GetTopSpendersAsync(limit);
            return Ok(topSpenders);
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/budget-status
        // Budget vs actual spending for each category
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("budget-status")]
        public async Task<ActionResult<BudgetStatusDto>> GetBudgetStatus()
        {
            _logger.LogInformation("Fetching budget status");

            var budgetStatus = await _reportService.GetBudgetStatusAsync();
            return Ok(budgetStatus);
        }

        // ═══════════════════════════════════════════════════════════════
        // GET api/dashboard/category-breakdown
        // Spending distribution by category (for pie charts)
        // ═══════════════════════════════════════════════════════════════

        [HttpGet("category-breakdown")]
        public async Task<ActionResult<CategoryBreakdownDto>> GetCategoryBreakdown()
        {
            _logger.LogInformation("Fetching category breakdown");

            var breakdown = await _reportService.GetCategoryBreakdownAsync();
            return Ok(breakdown);
        }
    }
}
