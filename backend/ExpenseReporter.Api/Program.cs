using ExpenseReporter.Api.Data;
using ExpenseReporter.Api.Interfaces;
using ExpenseReporter.Api.Middleware;
using ExpenseReporter.Api.Repositories;
using ExpenseReporter.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    // Enable automatic model validation error responses
    options.SuppressModelStateInvalidFilter = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with SQL Server and enable retry on failure
builder.Services.AddDbContext<AppDbContext>(options => { 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    );
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register repository and service
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ExcelExportService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
