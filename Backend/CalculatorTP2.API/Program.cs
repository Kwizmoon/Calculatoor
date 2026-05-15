using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CalculatorDatabase"));
builder.Services.AddScoped<Calculator>();

// SWAGGER SERVICES 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger for Azure (important!)
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();
app.Run();