# Backend CORS Configuration Guide

To allow your React frontend to communicate with the .NET API, you need to configure CORS (Cross-Origin Resource Sharing) in your backend.

## Quick Setup

Add this to your `Program.cs` file in the ExpenseReporter.Api project:

```csharp
// Add this AFTER var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",  // Vite dev server
                "http://localhost:3000"   // Alternative port
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// ... rest of your service configuration ...

var app = builder.Build();

// Add this BEFORE app.UseAuthorization();
app.UseCors("AllowFrontend");

// ... rest of your middleware ...
```

## Complete Example

Here's what your `Program.cs` should look like with CORS configured:

```csharp
using ExpenseReporter.Api.Data;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Repositories;
using ExpenseReporter.Api.Services;
using ExpenseReporter.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();

// Register services
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add CORS middleware - MUST be before UseAuthorization
app.UseCors("AllowFrontend");

// Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

## Testing CORS

After adding CORS configuration:

1. Restart your backend API
2. Start the frontend: `npm run dev`
3. Check the browser console for any CORS errors
4. If you still see errors, verify:
   - Backend is running on `https://localhost:7005`
   - Frontend is running on `http://localhost:5173`
   - CORS middleware is placed BEFORE `UseAuthorization()`

## Production Configuration

For production, update the CORS policy with your production frontend URL:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "https://your-production-domain.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});
```

## Common Issues

### Issue: "No 'Access-Control-Allow-Origin' header"
**Solution**: Make sure `app.UseCors("AllowFrontend")` is called BEFORE `app.UseAuthorization()`

### Issue: Credentials flag is not set
**Solution**: Add `.AllowCredentials()` to your CORS policy

### Issue: Pre-flight request failing
**Solution**: Ensure `.AllowAnyMethod()` is included in your policy

### Issue: Still getting CORS errors
**Solution**: 
1. Clear browser cache
2. Restart both frontend and backend
3. Check that the port numbers match exactly
4. Use browser dev tools Network tab to inspect the actual request/response headers