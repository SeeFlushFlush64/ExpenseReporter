using ExpenseReporter.Api.Data.DTOs;
using ExpenseReporter.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReporter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IReportService _service;

        public EmployeeController(IReportService service)
        {
            _service = service;
        }

        // ─── GET all employees ───────────────────────────────────
        // GET api/employee

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _service.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // ─── GET employee by ID ──────────────────────────────────
        // GET api/employee/1

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _service.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound($"Employee with Id {id} not found.");
            return Ok(employee);
        }

        // ─── POST create employee ────────────────────────────────
        // POST api/employee

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var employee = await _service.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        // ─── GET all categories ──────────────────────────────────
        // GET api/employee/categories

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryDto>>> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // ─── GET category by ID ──────────────────────────────────
        // GET api/employee/categories/1

        [HttpGet("categories/{id:int}")]
        public async Task<ActionResult<ExpenseCategoryDto>> GetCategoryById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);
            if (category == null) return NotFound($"Category with Id {id} not found.");
            return Ok(category);
        }

        // ─── POST create category ────────────────────────────────
        // POST api/employee/categories

        [HttpPost("categories")]
        public async Task<ActionResult<ExpenseCategoryDto>> CreateCategory([FromBody] ExpenseCategoryCreateDto dto)
        {
            var category = await _service.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
    }
}
