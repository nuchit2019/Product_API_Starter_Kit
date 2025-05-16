using ProductAPI.Application.Interfaces;
using ProductAPI.Application.Services;
using ProductAPI.Domain.Interfaces;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Middleware; 
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Register services
// TODO 01 [Register]: Implement ... Service  ...

// TODO 02 [Register]: Implement ... Repository ...


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Custom Middleware
// TODO 03 [Register]: Implement ... Middleware ...


app.MapControllers();

app.Run();
