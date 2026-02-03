# Expense Reporter API

A production-ready RESTful API for managing employee expenses, built with ASP.NET Core and Entity Framework Core.

## 🚀 Features

- **Complete CRUD operations** for expenses, employees, and expense categories
- **Advanced filtering** by employee, category, status, and date range
- **Excel export** functionality for expense reports
- **Input validation** with detailed error messages
- **Global exception handling** for consistent error responses
- **Structured logging** for monitoring and debugging
- **Swagger/OpenAPI documentation** for easy API exploration

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 8.0 Web API
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server (LocalDB for development)
- **Excel Generation:** EPPlus 7.x
- **API Documentation:** Swagger/Swashbuckle

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (comes with Visual Studio)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🏗️ Architecture

```
ExpenseReporter.Api/
├── Controllers/          # API endpoints
│   ├── ExpenseController.cs
│   └── EmployeeController.cs
├── Data/
│   ├── AppDbContext.cs   # EF Core DbContext
│   └── DTOs/             # Data Transfer Objects
├── Interfaces/           # Service & repository contracts
├── Middleware/           # Global exception handler
├── Models/               # Entity models
├── Repositories/         # Data access layer
└── Services/             # Business logic layer
```

### Design Patterns Used

- **Repository Pattern:** Abstracts data access logic
- **Dependency Injection:** Loose coupling between components
- **DTO Pattern:** Decouples API contracts from database entities
- **Middleware Pattern:** Global exception handling

## 🔧 Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/SeeFlushFlush64/ExpenseReporter.git
cd ExpenseReporter/backend
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Update Database Connection String (Optional)

Edit `appsettings.json` if you need a different connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ExpenseReporterDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Run Database Migrations

```bash
dotnet ef database update
```

This will:

- Create the `ExpenseReporterDb` database
- Create tables for Employees, ExpenseCategories, and Expenses
- Seed sample data (3 employees, 5 categories, 7 expenses)

### 5. Run the Application

```bash
dotnet run --project ExpenseReporter.Api
```

Or press **F5** in Visual Studio.

The API will start at `https://localhost:7005` (or the port shown in your console).

### 6. Explore with Swagger

Open your browser and navigate to:

```
https://localhost:7005/swagger
```

## 📡 API Endpoints

### Expenses

| Method   | Endpoint                                        | Description                                        |
| -------- | ----------------------------------------------- | -------------------------------------------------- |
| `GET`    | `/api/expense`                                  | Get all expenses                                   |
| `GET`    | `/api/expense/{id}`                             | Get expense by ID                                  |
| `GET`    | `/api/expense/employee/{employeeId}`            | Get expenses by employee                           |
| `GET`    | `/api/expense/category/{categoryId}`            | Get expenses by category                           |
| `GET`    | `/api/expense/status/{status}`                  | Get expenses by status (Pending/Approved/Rejected) |
| `GET`    | `/api/expense/daterange?startDate=&endDate=`    | Get expenses in date range                         |
| `GET`    | `/api/expense/summary/category`                 | Get expense totals grouped by category             |
| `GET`    | `/api/expense/summary/status`                   | Get expense counts grouped by status               |
| `GET`    | `/api/expense/total/employee/{employeeId}`      | Get total expenses for an employee                 |
| `GET`    | `/api/expense/total/category/{categoryId}`      | Get total expenses for a category                  |
| `GET`    | `/api/expense/export/excel?startDate=&endDate=` | Export expenses to Excel                           |
| `POST`   | `/api/expense`                                  | Create a new expense                               |
| `PUT`    | `/api/expense/{id}/status`                      | Update expense status                              |
| `DELETE` | `/api/expense/{id}`                             | Delete an expense                                  |

### Employees

| Method | Endpoint             | Description           |
| ------ | -------------------- | --------------------- |
| `GET`  | `/api/employee`      | Get all employees     |
| `GET`  | `/api/employee/{id}` | Get employee by ID    |
| `POST` | `/api/employee`      | Create a new employee |

### Expense Categories

| Method | Endpoint                        | Description           |
| ------ | ------------------------------- | --------------------- |
| `GET`  | `/api/employee/categories`      | Get all categories    |
| `GET`  | `/api/employee/categories/{id}` | Get category by ID    |
| `POST` | `/api/employee/categories`      | Create a new category |

## 📊 Sample Data

The database is seeded with:

- **3 Employees:** John Doe (IT), Jane Smith (Sales), Mike Johnson (Marketing)
- **5 Categories:** Travel, Meals, Supplies, Training, Software
- **7 Expenses:** Various expenses from January 2026

## 🔒 Input Validation

All `POST` requests are validated. Example validation rules:

- **Amount:** Must be greater than 0
- **Email:** Must be valid email format
- **Status:** Must be "Pending", "Approved", or "Rejected"
- **Description:** 1-500 characters

Invalid requests return `400 Bad Request` with detailed error messages.

## 📝 Example Usage

### Create an Expense

```bash
POST /api/expense
Content-Type: application/json

{
  "employeeId": 1,
  "categoryId": 2,
  "amount": 1500.00,
  "expenseDate": "2026-02-03",
  "description": "Team lunch meeting"
}
```

### Export Expenses to Excel

```bash
GET /api/expense/export/excel?startDate=2026-01-01&endDate=2026-01-31
```

Downloads an Excel file with formatted expense data.

## 🌐 Deployment (Azure)

### appsettings.Production.json

Create this file for production configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_AZURE_SQL_CONNECTION_STRING"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Deploy to Azure App Service

```bash
dotnet publish -c Release
# Follow Azure deployment steps for .NET apps
```

## 🧪 Testing

Test endpoints directly in Swagger UI or use tools like:

- [Postman](https://www.postman.com/)
- [Insomnia](https://insomnia.rest/)
- `curl` from command line

## 📁 Database Schema

```sql
Employees
├── Id (PK)
├── FirstName
├── LastName
├── Email
├── Department
└── HireDate

ExpenseCategories
├── Id (PK)
├── Name
├── Description
└── BudgetLimit

Expenses
├── Id (PK)
├── EmployeeId (FK)
├── CategoryId (FK)
├── Amount
├── ExpenseDate
├── Description
├── Status
└── CreatedAt
```

## 🤝 Contributing

This is a portfolio project. Feedback and suggestions are welcome!

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

## 👤 Author

**Michael Rhey Palaganas**

- GitHub: [@SeeFlushFlush64](https://github.com/SeeFlushFlush64)

## 🙏 Acknowledgments

- Built as part of a portfolio project to demonstrate ASP.NET Core skills
- Designed with production-ready patterns and best practices
