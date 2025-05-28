
using core.api.Middleware;
using core.category.Application.Interfaces;
using core.category.Application.Mappings;
using core.category.Application.Services;
using core.category.Domain.Interfaces;
using core.category.Infrastructure.Repositories;
using core.product.Application.Interfaces;
using core.product.Application.Services;
using core.product.Domain.Interfaces;
using core.product.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper
// วิธี 1: Register เฉพาะ Assembly ปัจจุบัน (ที่มี Profile)
//builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));

// วิธี 2: Register ทุก Assembly (ถ้ามีหลาย profile หลายโมดูล) 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Register services

//Product
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();


//Category
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
