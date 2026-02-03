# ExpenseTrack

A full-stack expense tracking and management system built with React and .NET Web API.

## Overview

ExpenseTrack is a comprehensive expense reporting solution that allows employees to submit expenses and administrators to review and approve them. The application features a clean, modern interface with dashboard analytics, filtering capabilities, and Excel export functionality.

## Screenshots

### Dashboard
<img width="1919" height="1036" alt="Screenshot 2026-02-03 191505" src="https://github.com/user-attachments/assets/a4e9e815-f7f1-4336-8f99-3530ecb98317" />
*Overview of expense reporting activity with key metrics and recent expenses*

### Expenses List
<img width="1919" height="1032" alt="Screenshot 2026-02-03 191516" src="https://github.com/user-attachments/assets/c7139504-696f-4507-8209-3a7d6cf940e9" />
*Manage and review all expense submissions with filtering and export capabilities*

### Add Expense
<img width="1919" height="1042" alt="Screenshot 2026-02-03 191524" src="https://github.com/user-attachments/assets/60455753-ea76-4dd9-aadc-39fc51843696" />
*Submit new expenses with detailed information*

### Employee Management
<img width="1919" height="1029" alt="Screenshot 2026-02-03 191533" src="https://github.com/user-attachments/assets/d91c1e04-e5c1-400f-ac5d-dea38d11d5d3" />
*View and manage employee information and expense totals*

### Excel Export
<img width="1919" height="1079" alt="Screenshot 2026-02-03 191605" src="https://github.com/user-attachments/assets/83546257-4966-44dc-afce-acb3fdebcc51" />
*Export expense reports to Excel with all details*

## Features

### Dashboard
- **Total Expenses Overview** - Real-time tracking of all submitted expenses
- **Pending Review Counter** - Number of expenses awaiting approval
- **Approved Expenses** - Track processed submissions
- **Monthly Summary** - Current month's expense activity
- **Recent Expenses Widget** - Quick view of latest submissions

### Expense Management
- **Submit Expenses** - Easy form-based expense submission
- **Search & Filter** - Filter by status, date range, and search terms
- **Status Tracking** - Monitor approval status (Pending/Approved)
- **Excel Export** - Export filtered expenses to Excel format
- **Detailed View** - View all expense details including employee, category, amount, and date

### Employee Management
- **Employee Directory** - View all employees with their information
- **Department Tracking** - Organize employees by department
- **Expense Totals** - See total expenses per employee
- **Search Functionality** - Find employees by name, email, or department

## Tech Stack

### Frontend
- **React** - UI framework
- **React Router** - Navigation and routing
- **Axios** - HTTP client for API requests
- **Modern CSS** - Responsive design and styling

### Backend
- **.NET Web API** - RESTful API
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Database (assumed)
- **EPPlus** - Excel generation library

## Project Structure

```
ExpenseTrack/
├── client/                 # React frontend
│   ├── src/
│   │   ├── components/    # Reusable components
│   │   ├── pages/         # Page components
│   │   │   ├── Dashboard.jsx
│   │   │   ├── Expenses.jsx
│   │   │   ├── AddExpense.jsx
│   │   │   └── Employees.jsx
│   │   ├── services/      # API service layer
│   │   ├── App.jsx
│   │   └── index.js
│   └── package.json
│
└── server/                # .NET Web API
    ├── Controllers/
    ├── Models/
    ├── Services/
    ├── Data/
    └── Program.cs
```

## Getting Started

### Prerequisites
- Node.js (v14 or higher)
- .NET 6.0 SDK or higher
- SQL Server or SQL Server Express

### Installation

#### Backend Setup
1. Navigate to the server directory
   ```bash
   cd server
   ```

2. Restore NuGet packages
   ```bash
   dotnet restore
   ```

3. Update the connection string in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=ExpenseTrackDB;Trusted_Connection=true;"
   }
   ```

4. Run database migrations
   ```bash
   dotnet ef database update
   ```

5. Start the API
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:5173` (or configured port)

#### Frontend Setup
1. Navigate to the client directory
   ```bash
   cd client
   ```

2. Install dependencies
   ```bash
   npm install
   ```

3. Update the API base URL in your configuration file if needed

4. Start the development server
   ```bash
   npm start
   ```
   The application will open at `http://localhost:5173`

## API Endpoints

### Expenses
- `GET /api/expenses` - Get all expenses with optional filtering
- `GET /api/expenses/{id}` - Get expense by ID
- `POST /api/expenses` - Create new expense
- `PUT /api/expenses/{id}` - Update expense
- `DELETE /api/expenses/{id}` - Delete expense
- `PUT /api/expenses/{id}/status` - Update expense status
- `GET /api/expenses/export` - Export expenses to Excel

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

### Dashboard
- `GET /api/dashboard/stats` - Get dashboard statistics

## Database Schema

### Expenses Table
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `Category` (string)
- `Amount` (decimal)
- `Date` (datetime)
- `Description` (string)
- `Status` (string) - "Pending" or "Approved"
- `Created` (datetime)

### Employees Table
- `Id` (int, PK)
- `Name` (string)
- `Email` (string)
- `Department` (string)
- `HireDate` (datetime)

## Features in Detail

### Expense Submission
Employees can submit expenses with the following information:
- Employee selection
- Expense category (Meals, Travel, Training, Software, Supplies, etc.)
- Amount
- Date
- Detailed description (up to 500 characters)

### Approval Workflow
- Expenses are submitted with "Pending" status
- Administrators can review and approve expenses
- Status updates are tracked with timestamps

### Reporting
- Export filtered expense data to Excel
- Filter by status and date range
- Search across multiple fields
- Real-time dashboard statistics

## Configuration

### Environment Variables
Create a `.env` file in the client directory:
```
REACT_APP_API_URL=http://localhost:5173
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Built with React and .NET
- UI inspired by modern expense management solutions
- Excel export powered by EPPlus

## Support

For issues and questions, please open an issue on GitHub or contact the development team.

---

**Note**: Remember to update the database connection string and API URLs before deploying to production.
