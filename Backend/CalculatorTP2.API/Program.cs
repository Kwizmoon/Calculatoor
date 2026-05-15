using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS - communicate with frontend
builder.Services.AddCors();

builder.Services.AddControllers();

// 2. DATABASE CONFIGURATION
// This looks for "DefaultConnection" in appsettings.json or Azure environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Force PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<Calculator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger  for testing Login endpoints
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();
app.Run();