# 🚀 Quick Start Guide - ExpenseTrack Frontend

## What You've Got

A modern, professional React frontend for your ExpenseReporter API with:
- ✨ Distinctive design (no generic AI look!)
- 📊 Dashboard with statistics
- 📋 Filterable expense list
- ➕ Add expense form with validation
- 👥 Employee directory
- 📥 Excel export functionality
- 🎨 Custom design system using Manrope font and a coral red accent

## Setup Steps

### 1. Open Your Project
```bash
cd /path/to/expense-reporter-frontend
```

### 2. Install Dependencies
```bash
npm install
```

### 3. Configure Backend CORS
**IMPORTANT**: Your .NET backend needs to allow requests from the frontend.

Open `ExpenseReporter.Api/Program.cs` and add:

```csharp
// After: var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// After: var app = builder.Build();
// And BEFORE: app.UseAuthorization();
app.UseCors("AllowFrontend");
```

See `CORS-SETUP.md` for detailed instructions.

### 4. Start Your Backend
Make sure your ExpenseReporter API is running on `https://localhost:7005`

```bash
cd /path/to/ExpenseReporter/backend
dotnet run --project ExpenseReporter.Api
```

### 5. Start the Frontend
```bash
npm run dev
```

Visit: `http://localhost:5173`

## First Look

When you open the app, you'll see:
- **Sidebar navigation** with Dashboard, Expenses, Add Expense, and Employees
- **Dashboard** showing total expenses, pending count, approved count
- **Recent expenses** table

## Key Features to Try

1. **Dashboard** - See your expense overview and stats
2. **Expenses Page** - Filter by status, search, date range, export to Excel
3. **Add Expense** - Create a new expense (validates input)
4. **Employees Page** - View all employees with their total expenses

## Design Highlights

### What Makes This Different?

Unlike typical AI-generated interfaces, this design features:
- **Custom typography**: Manrope (not Inter/Roboto!)
- **Distinctive colors**: Coral red (#E63946) instead of generic purple
- **Thoughtful spacing**: Consistent, breathable layout
- **Smooth animations**: Subtle, purposeful transitions
- **Professional finish**: Every detail considered

### Color Palette
- Accent: #E63946 (Coral Red)
- Success: #06B6D4 (Cyan)
- Warning: #F59E0B (Amber)
- Danger: #EF4444 (Red)

## Troubleshooting

### "Failed to load dashboard data"
- ✅ Backend is running on port 7005
- ✅ CORS is configured in Program.cs
- ✅ Database has seed data

### CORS Errors in Console
- Check `CORS-SETUP.md` for detailed configuration
- Ensure CORS middleware comes BEFORE `UseAuthorization()`
- Restart both frontend and backend after changes

### Port Already in Use
```bash
npm run dev -- --port 3000
```
(Then update CORS to include port 3000)

## Customization

### Change Colors
Edit `src/App.css` CSS variables:
```css
:root {
  --color-accent: #YOUR_COLOR;
}
```

### Change API URL
Edit `API_BASE_URL` in each page component if your backend runs on a different port.

### Change Fonts
Update Google Fonts import in `src/App.css` and CSS variables.

## Next Steps

1. ✅ Set up CORS on backend
2. ✅ Start both servers
3. ✅ Test all features
4. 🎨 Customize colors/fonts if desired
5. 🚀 Build for production: `npm run build`

## Need Help?

Check these files:
- `README.md` - Comprehensive documentation
- `CORS-SETUP.md` - Backend CORS configuration
- Project structure in `src/` folder

## Production Build

When ready to deploy:
```bash
npm run build
```

Outputs to `dist/` folder - upload to Netlify, Vercel, or your hosting service.

---

Enjoy your professional, distinctive expense tracker! 🎉