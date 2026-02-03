using ExpenseReporter.Api.Data.DTOs;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReporter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IReportService _service;
        private readonly ExcelExportService _excelService;

        public ExpenseController(IReportService service, ExcelExportService excelService)
        {
            _service = service;
            _excelService = excelService;
        }

        // ─── GET all expenses ────────────────────────────────────
        // GET api/expense

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAllExpenses()
        {
            var expenses = await _service.GetAllExpensesAsync();
            return Ok(expenses);
        }

        // ─── GET expense by ID ───────────────────────────────────
        // GET api/expense/1

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExpenseDto>> GetExpenseById(int id)
        {
            var expense = await _service.GetExpenseByIdAsync(id);
            if (expense == null) return NotFound($"Expense with Id {id} not found.");
            return Ok(expense);
        }

        // ─── GET expenses by employee ────────────────────────────
        // GET api/expense/employee/1

        [HttpGet("employee/{employeeId:int}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByEmployee(int employeeId)
        {
            var expenses = await _service.GetExpensesByEmployeeIdAsync(employeeId);
            return Ok(expenses);
        }

        // ─── GET expenses by category ────────────────────────────
        // GET api/expense/category/1

        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByCategory(int categoryId)
        {
            var expenses = await _service.GetExpensesByCategoryIdAsync(categoryId);
            return Ok(expenses);
        }

        // ─── GET expenses by status ──────────────────────────────
        // GET api/expense/status/Approved

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByStatus(string status)
        {
            var expenses = await _service.GetExpensesByStatusAsync(status);
            return Ok(expenses);
        }

        // ─── GET expenses by date range ──────────────────────────
        // GET api/expense/daterange?startDate=2026-01-01&endDate=2026-01-31

        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var expenses = await _service.GetExpensesByDateRangeAsync(startDate, endDate);
            return Ok(expenses);
        }

        // ─── GET summary by category ─────────────────────────────
        // GET api/expense/summary/category

        [HttpGet("summary/category")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetSummaryByCategory()
        {
            var summary = await _service.GetExpenseSummaryByCategoryAsync();
            return Ok(summary);
        }

        // ─── GET count by status ─────────────────────────────────
        // GET api/expense/summary/status

        [HttpGet("summary/status")]
        public async Task<ActionResult<Dictionary<string, long>>> GetCountByStatus()
        {
            var counts = await _service.GetExpenseCountByStatusAsync();
            return Ok(counts);
        }

        // ─── GET total by employee ───────────────────────────────
        // GET api/expense/total/employee/1

        [HttpGet("total/employee/{employeeId:int}")]
        public async Task<ActionResult<decimal>> GetTotalByEmployee(int employeeId)
        {
            var total = await _service.GetTotalExpensesByEmployeeAsync(employeeId);
            return Ok(total);
        }

        // ─── GET total by category ───────────────────────────────
        // GET api/expense/total/category/1

        [HttpGet("total/category/{categoryId:int}")]
        public async Task<ActionResult<decimal>> GetTotalByCategory(int categoryId)
        {
            var total = await _service.GetTotalExpensesByCategoryAsync(categoryId);
            return Ok(total);
        }

        // ─── EXPORT to Excel ─────────────────────────────────────
        // GET api/expense/export/excel?startDate=2026-01-01&endDate=2026-01-31

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            IEnumerable<ExpenseDto> expenses;

            if (startDate.HasValue && endDate.HasValue)
            {
                expenses = await _service.GetExpensesByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                expenses = await _service.GetAllExpensesAsync();
            }

            var excelData = _excelService.GenerateExpenseReport(expenses, startDate, endDate);

            var fileName = startDate.HasValue && endDate.HasValue
                ? $"ExpenseReport_{startDate.Value:yyyyMMdd}_{endDate.Value:yyyyMMdd}.xlsx"
                : $"ExpenseReport_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // ─── POST create expense ─────────────────────────────────
        // POST api/expense

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> CreateExpense([FromBody] ExpenseCreateDto dto)
        {
            var expense = await _service.CreateExpenseAsync(dto);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        // ─── PUT update expense status ───────────────────────────
        // PUT api/expense/1/status

        [HttpPut("{id:int}/status")]
        public async Task<ActionResult<ExpenseDto>> UpdateExpenseStatus(int id, [FromBody] ExpenseUpdateStatusDto dto)
        {
            try
            {
                var expense = await _service.UpdateExpenseStatusAsync(id, dto);
                return Ok(expense);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ─── DELETE expense ──────────────────────────────────────
        // DELETE api/expense/1

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var deleted = await _service.DeleteExpenseAsync(id);
            if (!deleted) return NotFound($"Expense with Id {id} not found.");
            return NoContent();
        }
    }
}
