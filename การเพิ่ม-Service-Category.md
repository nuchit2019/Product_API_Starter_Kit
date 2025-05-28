# เพิ่ม **Service Category** เข้าไปในโปรเจกต์ ดังนี้:

---

# 📦 เพิ่ม Service `core.category` เข้า Solution core.final-x.sln

คู่มือนี้จะแนะนำทีละขั้นตอนสำหรับมือใหม่ในการเพิ่ม Service ใหม่ `Category` เข้าไปในระบบเดิมที่มี `Product Service` อยู่แล้ว โดยใช้แนวทาง Clean Architecture

---

## ✅ โครงสร้าง Solution เดิม

```
ProductAPI-final-x/
├── core.final-x.sln
└── src/
    ├── core.api/
    ├── core.product/
    │   ├── core.product.application/
    │   ├── core.product.domain/
    │   ├── core.product.infrastructure/
    │   └── core.product.persistence/
```

---

## 🧩 เป้าหมาย

เพิ่ม Service `core.category` แบบครบถ้วน ประกอบด้วย:

```
ProductAPI-final-x/
├── core.final-x.sln
└── src/
    ├── core.api/
    ├── core.product/
    │   ├── core.product.application/
    │   ├── core.product.domain/
    │   ├── core.product.infrastructure/
    ├── ***core.category***/
    │   ├── core.category.application/
    │   ├── core.category.domain/
    │   ├── core.category.infrastructure/ 
```

---

## 🚀 ขั้นตอนที่ 1: เพิ่ม Project ใหม่

ให้เพิ่ม 4 Project สำหรับ `Category`

```bash
cd src
mkdir core.category
cd core.category

dotnet new classlib -n core.category.domain
dotnet new classlib -n core.category.application
dotnet new classlib -n core.category.infrastructure
dotnet new classlib -n core.category.persistence
```

จากนั้นเพิ่มทั้ง 4 Project เข้าไปใน Solution

```bash
dotnet sln ../../core.final-x.sln add core.category.*
```

---

## 🔁 ขั้นตอนที่ 2: เชื่อมโยง Project References

ตั้งค่าการอ้างอิงระหว่าง Layer:

```bash
dotnet add core.category.application reference core.category.domain
dotnet add core.category.infrastructure reference core.category.application core.category.domain
dotnet add core.category.persistence reference core.category.application core.category.domain
```

ใน `core.api` ให้เพิ่ม Reference:

```bash
dotnet add ../core.api/core.api.csproj reference ../core.category.infrastructure/core.category.infrastructure.csproj
```

---

## 🧠 ขั้นตอนที่ 3: สร้าง Class และ Layer

### 3.1 `core.category.domain`

```csharp
namespace Core.Category.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### 3.2 `core.category.application`

* Interface `ICategoryService`
* DTO: `CreateCategoryRequest`, `CategoryDto`

### 3.3 `core.category.infrastructure`

* Implement `ICategoryService`
* Add Business Logic

### 3.4 `core.category.persistence`

* สร้าง `ICategoryRepository`, `CategoryRepository` (ใช้ Dapper หรือ EF Core)
* เชื่อมต่อ DB MSSQL

---

## 🌐 ขั้นตอนที่ 4: เพิ่ม Endpoint ใน `core.api`

ใน `core.api/Controllers/CategoryController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request)
    {
        await _service.CreateAsync(request);
        return Ok();
    }
}
```

---

## 🛠 ขั้นตอนที่ 5: ลงทะเบียน DI ใน `Program.cs`

```csharp
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
```

---

## 🧪 ขั้นตอนที่ 6: ทดสอบ API

ใน `core.api/api.http` หรือ Postman:

### 🔹 สร้างหมวดหมู่สินค้า

```http
POST http://localhost:5000/api/category
Content-Type: application/json

{
  "name": "Notebook",
  "description": "สินค้าประเภทโน้ตบุ๊ก"
}
```

### 🔹 ดึงรายการทั้งหมด

```http
GET http://localhost:5000/api/category
```

---

## ✅ ทดสอบและตรวจสอบผลลัพธ์

* ทดสอบ API ผ่าน Swagger / Postman
* ตรวจสอบการบันทึกข้อมูลในฐานข้อมูล
* ตรวจสอบ Logs

---

## 📌 สรุป

คุณได้เพิ่ม Service ใหม่แบบครบวงจรในแนวทาง Clean Architecture ประกอบด้วย:

* Domain → Application → Infrastructure → Persistence → API
* แยกแต่ละชั้นชัดเจน ทำให้ดูแลง่าย และสามารถเขียน Unit Test ได้ในอนาคต

---
 
