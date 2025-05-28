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
********************************************
    ├── core.category/
    │   ├── core.category.application/
    │   ├── core.category.domain/
    │   ├── core.category.infrastructure/ 
```

---

## 🚀 ขั้นตอนที่ 1: เพิ่ม Project ใหม่

ให้เพิ่ม  Project สำหรับ `Category`

## Starter:

ให้ Download Multi Project...Final-x ... Starter Add Service Category
https://github.com/nuchit2019/Product_API_Starter_Kit/tree/Final-x

## Final:

Multi Project...Final-x2 ... Code final Service Category 
https://github.com/nuchit2019/Product_API_Starter_Kit/tree/Final-x2

```bash
git clone -b Final-x2 https://github.com/nuchit2019/Product_API_Starter_Kit.git
```

#

แตก Zip ... เปิด Solution ...
เพิ่ม Project Class Library ... ดังรูป

![image](https://github.com/user-attachments/assets/c96c40c4-e798-4bf7-b397-db5a050cbb83)


---

## 🔁 ขั้นตอนที่ 2: เชื่อมโยง Project References

ตั้งค่าการอ้างอิงระหว่าง Layer:
core.api --> core.category

 ![image](https://github.com/user-attachments/assets/06287454-14e8-412e-806f-a7a095aaee2c)


---

## ✅  ขั้นตอนที่ 3: SQ สร้างตาราง `Category` บน **Microsoft SQL Server (MSSQL)** ที่เหมาะกับระบบสินค้าทั่วไป (สามารถปรับแต่งคอลัมน์เพิ่มตามต้องการ):

```sql
CREATE TABLE Category (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,  -- Primary Key, Auto Increment
    Name NVARCHAR(100) NOT NULL,               -- ชื่อหมวดหมู่ (จำเป็น)
    Description NVARCHAR(255) NULL,            -- รายละเอียด (ไม่บังคับ)
    IsActive BIT NOT NULL DEFAULT 1,           -- สถานะใช้งาน (1=ใช้งาน, 0=ไม่ใช้งาน)
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- วันเวลาที่สร้าง
    UpdatedAt DATETIME NULL                    -- วันเวลาที่อัปเดตล่าสุด
);
```

## ✅  ขั้นตอนที่ 4: ถ้าต้องการ Foreign Key กับ Product (กรณีหนึ่งสินค้าต่อหนึ่งหมวดหมู่):

```sql
ALTER TABLE Product
ADD CategoryId INT NOT NULL
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId);
```


## ✅  ขั้นตอนที่ 5: สร้าง Class และ Layer

### 5.1 `core.category.domain`

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

### ✅ 5.2 `core.category.application`

* Interface `ICategoryService`
* DTO: `CreateCategoryRequest`, `CategoryDto`

### ✅ 5.3 `core.category.infrastructure`

* Implement `ICategoryService`
* Add Business Logic

### ✅ 5.4 `core.category.persistence`

* สร้าง `ICategoryRepository`, `CategoryRepository` (ใช้ Dapper)
* เชื่อมต่อ DB MSSQL

---

## 🌐 ขั้นตอนที่ 6: เพิ่ม Endpoint ใน `core.api`

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

## 🛠 ขั้นตอนที่ 7: ลงทะเบียน DI ใน `Program.cs`

```csharp
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
```

---

## 🧪 ขั้นตอนที่ 8: ทดสอบ API

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
 
