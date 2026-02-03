using ExpenseReporter.Api.Data.DTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ExpenseReporter.Api.Services
{
    public class ExcelExportService
    {
        private readonly ILogger<ExcelExportService> _logger;

        public ExcelExportService(ILogger<ExcelExportService> logger)
        {
            _logger = logger;
            // Set EPPlus license context (required for EPPlus 5.0+)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] GenerateExpenseReport(IEnumerable<ExpenseDto> expenses, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Generating Excel export for {Count} expenses", expenses.Count());

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Expense Report");

            // Add title
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1"].Value = "Expense Report";
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Add date range if provided
            if (startDate.HasValue && endDate.HasValue)
            {
                worksheet.Cells["A2:H2"].Merge = true;
                worksheet.Cells["A2"].Value = $"Period: {startDate.Value:yyyy-MM-dd} to {endDate.Value:yyyy-MM-dd}";
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2"].Style.Font.Italic = true;
            }

            // Add headers
            var headerRow = startDate.HasValue ? 4 : 3;
            var headers = new[] { "ID", "Employee", "Category", "Amount", "Date", "Description", "Status", "Created" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[headerRow, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Add data
            var expenseList = expenses.ToList();
            var dataStartRow = headerRow + 1;
            for (int i = 0; i < expenseList.Count; i++)
            {
                var expense = expenseList[i];
                var row = dataStartRow + i;

                worksheet.Cells[row, 1].Value = expense.Id;
                worksheet.Cells[row, 2].Value = expense.EmployeeName;
                worksheet.Cells[row, 3].Value = expense.CategoryName;
                worksheet.Cells[row, 4].Value = expense.Amount;
                worksheet.Cells[row, 4].Style.Numberformat.Format = "₱#,##0.00";
                worksheet.Cells[row, 5].Value = expense.ExpenseDate;
                worksheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-mm-dd";
                worksheet.Cells[row, 6].Value = expense.Description;
                worksheet.Cells[row, 7].Value = expense.Status;
                worksheet.Cells[row, 8].Value = expense.CreatedAt;
                worksheet.Cells[row, 8].Style.Numberformat.Format = "yyyy-mm-dd hh:mm";

                // Color-code status
                var statusCell = worksheet.Cells[row, 7];
                statusCell.Style.Font.Bold = true;
                switch (expense.Status)
                {
                    case "Approved":
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Green);
                        break;
                    case "Rejected":
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        break;
                    case "Pending":
                        statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Orange);
                        break;
                }
            }

            // Add summary section
            var summaryRow = dataStartRow + expenseList.Count + 2;
            worksheet.Cells[summaryRow, 3].Value = "Total:";
            worksheet.Cells[summaryRow, 3].Style.Font.Bold = true;
            worksheet.Cells[summaryRow, 4].Formula = $"=SUM(D{dataStartRow}:D{dataStartRow + expenseList.Count - 1})";
            worksheet.Cells[summaryRow, 4].Style.Font.Bold = true;
            worksheet.Cells[summaryRow, 4].Style.Numberformat.Format = "₱#,##0.00";
            worksheet.Cells[summaryRow, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[summaryRow, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Set minimum column widths
            for (int col = 1; col <= 8; col++)
            {
                if (worksheet.Column(col).Width < 12)
                    worksheet.Column(col).Width = 12;
            }

            _logger.LogInformation("Excel export generated successfully");

            return package.GetAsByteArray();
        }
    }
}
