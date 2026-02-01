using Microsoft.EntityFrameworkCore;
using ExpenseReporter.Api.Models;

namespace ExpenseReporter.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Employee)
            .WithMany(emp => emp.Expenses)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                Department = "IT",
                HireDate = new DateTime(2020, 1, 15)
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@company.com",
                Department = "Sales",
                HireDate = new DateTime(2019, 6, 1)
            },
            new Employee
            {
                Id = 3,
                FirstName = "Mike",
                LastName = "Johnson",
                Email = "mike.johnson@company.com",
                Department = "Marketing",
                HireDate = new DateTime(2021, 3, 20)
            }
        );

        // Seed Categories
        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory
            {
                Id = 1,
                Name = "Travel",
                Description = "Transportation and accommodation",
                BudgetLimit = 50000
            },
            new ExpenseCategory
            {
                Id = 2,
                Name = "Meals",
                Description = "Food and beverages",
                BudgetLimit = 20000
            },
            new ExpenseCategory
            {
                Id = 3,
                Name = "Supplies",
                Description = "Office supplies and materials",
                BudgetLimit = 15000
            },
            new ExpenseCategory
            {
                Id = 4,
                Name = "Training",
                Description = "Professional development and courses",
                BudgetLimit = 30000
            },
            new ExpenseCategory
            {
                Id = 5,
                Name = "Software",
                Description = "Software licenses and subscriptions",
                BudgetLimit = 25000
            }
        );

        // Seed Expenses (January 2026)
        modelBuilder.Entity<Expense>().HasData(
            new Expense
            {
                Id = 1,
                EmployeeId = 1,
                CategoryId = 1,
                Amount = 5500,
                ExpenseDate = new DateTime(2026, 1, 5),
                Description = "Flight to client site in Cebu",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 5)
            },
            new Expense
            {
                Id = 2,
                EmployeeId = 1,
                CategoryId = 2,
                Amount = 850,
                ExpenseDate = new DateTime(2026, 1, 6),
                Description = "Team lunch with client",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 6)
            },
            new Expense
            {
                Id = 3,
                EmployeeId = 2,
                CategoryId = 3,
                Amount = 3200,
                ExpenseDate = new DateTime(2026, 1, 10),
                Description = "Office supplies and materials",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 10)
            },
            new Expense
            {
                Id = 4,
                EmployeeId = 2,
                CategoryId = 1,
                Amount = 12000,
                ExpenseDate = new DateTime(2026, 1, 15),
                Description = "Conference travel to Manila",
                Status = "Pending",
                CreatedAt = new DateTime(2026, 1, 15)
            },
            new Expense
            {
                Id = 5,
                EmployeeId = 3,
                CategoryId = 4,
                Amount = 15000,
                ExpenseDate = new DateTime(2026, 1, 20),
                Description = "Digital marketing workshop",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 20)
            },
            new Expense
            {
                Id = 6,
                EmployeeId = 1,
                CategoryId = 5,
                Amount = 8500,
                ExpenseDate = new DateTime(2026, 1, 22),
                Description = "Annual IDE license renewal",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 22)
            },
            new Expense
            {
                Id = 7,
                EmployeeId = 3,
                CategoryId = 2,
                Amount = 1200,
                ExpenseDate = new DateTime(2026, 1, 25),
                Description = "Client dinner meeting",
                Status = "Approved",
                CreatedAt = new DateTime(2026, 1, 25)
            }
        );
    }
}