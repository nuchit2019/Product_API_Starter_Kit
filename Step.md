เพื่อจัดทำ **คู่มือการเขียน Code ตามโครงสร้าง Project: ProductAPI** โดยอิงหลัก **Clean Architecture**, **SOLID Principles** และแนวทาง **Clean Code**, ขั้นตอนต่อไปนี้:

#

## 🛠️ คู่มือสร้างระบบ Product API ด้วย .NET 8 + Dapper + Clean Architecture

### 🔧 Stack ที่ใช้

* ASP.NET Core 8 Web API
* Dapper (Micro ORM)
* MSSQL LocalDB
* Serilog (Logging)
* Clean Architecture
* SOLID Principles
* Middleware (Exception Handling, Logging, Response Wrapping)

#

## 1️⃣ โครงสร้างโฟลเดอร์ (จาก Project แนบมา)

```plaintext
ProductAPI/
│
├── ProductAPI.Application        // Business Logic
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
│
├── ProductAPI.Domain            // Entity (Business Model)
│
├── ProductAPI.Infrastructure    // Data Access (Repositories, Dapper)
│
├── ProductAPI.Presentation.WebAPI  // API Layer
│   └── Controllers/
│
├── ProductAPI.Shared            // Shared Models (ApiResponse, Enums, Exceptions)
│
└── ProductAPI.Tests             // (Optional) Unit Tests
```

#

## 2️⃣ สร้าง Database และ Table

### 📦 Database: MSSQL LocalDB

```sql
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
```

#

## 3️⃣ สร้าง Project แบบ Step-by-Step

### ✅ Step 1: สร้าง Solution และ Project

```bash
dotnet new sln -n ProductAPI
cd ProductAPI

dotnet new classlib -n ProductAPI.Domain
dotnet new classlib -n ProductAPI.Application
dotnet new classlib -n ProductAPI.Infrastructure
dotnet new classlib -n ProductAPI.Shared
dotnet new webapi   -n ProductAPI.Presentation.WebAPI

dotnet sln add ./ProductAPI.Domain
dotnet sln add ./ProductAPI.Application
dotnet sln add ./ProductAPI.Infrastructure
dotnet sln add ./ProductAPI.Shared
dotnet sln add ./ProductAPI.Presentation.WebAPI
```

**🔁 อ้างอิง Project กัน:**

* `Presentation.WebAPI` → Reference ทุก Layer
* `Application` → Reference `Domain`, `Shared`
* `Infrastructure` → Reference `Application`, `Domain`, `Shared`

#

## 4️⃣ Step-by-Step การเขียน Code (พร้อมอธิบายหลัก SOLID และ Clean Code)

### ✅ 4.1 สร้าง Entity (Domain Layer)

📁 `ProductAPI.Domain/Entities/Product.cs`

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

**🧠 หลักการ:**

* **S (Single Responsibility)**: Entity นี้มีหน้าที่แทนข้อมูลเท่านั้น
* Clean Code: ชื่อ class/field ชัดเจน, ใช้ type ที่เหมาะสม

#

### ✅ 4.2 สร้าง DTO (Application Layer)

📁 `ProductAPI.Application/DTOs/ProductDto.cs`

```csharp
public record ProductDto(int Id, string Name, string? Description, decimal Price, int Stock);
```

**🧠 หลักการ:**

* **I (Interface Segregation)**: แยก DTO ไม่ให้ใช้ Entity ตรง ๆ
* ใช้ `record` เพื่อความกระชับ, immutable

#

### ✅ 4.3 สร้าง Interface + Service (Application Layer)

📁 `Interfaces/IProductService.cs`

```csharp
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductDto product);
    Task<bool> UpdateAsync(int id, ProductDto product);
    Task<bool> DeleteAsync(int id);
}
```

📁 `Services/ProductService.cs`

```csharp
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    // ใช้หลัก Clean Code และ DRY Principle
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
        => (await _repository.GetAllAsync())
           .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock));

    // ... (อื่น ๆ คล้ายกัน)
}
```

**🧠 หลักการ:**

* **D (Dependency Inversion)**: ใช้ Interface แทนการผูกตรง
* **O (Open/Closed)**: แก้ Service ได้โดยไม่กระทบ Layer อื่น
* Clean Code: Function name ชัดเจน, ใช้ LINQ แปลงข้อมูล

---

### ✅ 4.4 สร้าง Repository (Infrastructure Layer)

📁 `Interfaces/IProductRepository.cs`

```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}
```

📁 `Repositories/ProductRepository.cs`

```csharp
public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public ProductRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        const string query = "SELECT * FROM Products";
        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<Product>(query);
    }

    // ... (อื่น ๆ)
}
```

**🧠 หลักการ:**

* **S (Single Responsibility)**: จัดการเฉพาะ Data Access
* Clean Code: ใช้ `const string`, ใช้ `using`

#

### ✅ 4.5 Controller (API Layer)

📁 `Controllers/ProductsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(result));
    }

    // ... POST, PUT, DELETE
}
```

**🧠 หลักการ:**

* **D (Dependency Inversion)**: Controller ไม่ขึ้นกับ Implementation
* Clean Code: Response เป็น `ApiResponse`, แยก logic ไป Service

#

### ✅ 4.6 Middleware + ApiResponse (Shared)

📁 `Shared/ApiResponse.cs`

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data) => new() { Success = true, Data = data };
}
```

📁 `Middleware/ExceptionMiddleware.cs`

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ApiResponse<string>.Fail("Internal Server Error"));
        }
    }
}
```

#

## 🔚 สรุปแนวทางและ SOLID

| หลักการ | ตัวอย่างในระบบ                                                  |
| ------- | --------------------------------------------------------------- |
| **S**   | `Product`, `ProductService`, `ProductRepository` มีหน้าที่เฉพาะ |
| **O**   | เปิดให้ต่อยอดผ่าน Interface, Middleware                         |
| **L**   | ทุก Class สามารถแทนที่กันได้ผ่าน Interface                      |
| **I**   | ไม่ยัด method ที่ไม่จำเป็นใน Interface                          |
| **D**   | Controller ไม่ขึ้นตรงกับ Implementation (ใช้ DI)                |

#✅
