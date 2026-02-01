using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpenseReporter.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "FirstName", "HireDate", "LastName" },
                values: new object[,]
                {
                    { 1, "IT", "john.doe@company.com", "John", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe" },
                    { 2, "Sales", "jane.smith@company.com", "Jane", new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith" },
                    { 3, "Marketing", "mike.johnson@company.com", "Mike", new DateTime(2021, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johnson" }
                });

            migrationBuilder.InsertData(
                table: "ExpenseCategories",
                columns: new[] { "Id", "BudgetLimit", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 50000m, "Transportation and accommodation", "Travel" },
                    { 2, 20000m, "Food and beverages", "Meals" },
                    { 3, 15000m, "Office supplies and materials", "Supplies" },
                    { 4, 30000m, "Professional development and courses", "Training" },
                    { 5, 25000m, "Software licenses and subscriptions", "Software" }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Description", "EmployeeId", "ExpenseDate", "Status" },
                values: new object[,]
                {
                    { 1, 5500m, 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flight to client site in Cebu", 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 2, 850m, 2, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team lunch with client", 1, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 3, 3200m, 3, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Office supplies and materials", 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 4, 12000m, 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conference travel to Manila", 2, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 5, 15000m, 4, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital marketing workshop", 3, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 6, 8500m, 5, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual IDE license renewal", 1, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 7, 1200m, 2, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Client dinner meeting", 3, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryId",
                table: "Expenses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EmployeeId",
                table: "Expenses",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");
        }
    }
}
