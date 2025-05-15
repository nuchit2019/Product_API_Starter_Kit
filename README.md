# 🚀 จากมือใหม่...สู่มือโปร!...

**Starter Kit สำหรับ Dev สาย .NET: CRUD Product API + Clean Architecture + SOLID Principles + Best Practices**

> เรียนรู้จากโครงสร้างจริง ฝึกจากโปรเจกต์จริง เพื่อทักษะที่ใช้งานได้จริง
#

# **`CRUD Product API Starter Kit`** สำหรับนักพัฒนาใหม่(New Back-end development (Dev Back-end)) สาย .Net สำหรับฝึกฝีมือ... โปรเจกต์นี้... เน้นอธิบายโครงสร้าง Clean Architecture..., Clean Code..., SOLID Principles..., วิธีรัน..., และ Best Practices...ในแบบที่ง่ายที่สุด...เพื่อให้การเริ่มต้นนั้นเต็มไปด้วยความสนุก และประสบความสำเร็จ... ที่จะทำให้คุณเป็น มืออาชีพ ในเร็ววัน จะช้า... หรือ เร็ว... ขึ้นอยู่กับ การฝึกซ้อม...และซ้อม...และซ้อม... :

### Branch: Starter คุณควรเริ่มที่นี่:
https://github.com/nuchit2019/Product_API_Starter_Kit/tree/Starter

### Branch: Final ถ้าคุณติดปัญหา... ไปแอบดูตัวอย่างที่นี่ ได้...
https://github.com/nuchit2019/Product_API_Starter_Kit/tree/Final

### เอกสาร: ความรู้พื้นฐานและเครื่องมือที่ต้องใช้ 
https://github.com/nuchit2019/Product_API_Starter_Kit/blob/main/Document.md
#

# และนี่คือภาพรวม ... 
 
# 🧱 ProductAPI(.NET 8 + Dapper + Clean Architecture)

> RESTful API สำหรับจัดการสินค้า (Product CRUD)  
> พัฒนาโดยใช้ C#, ASP.NET Core 8, Dapper, Serilog, และ Clean Architecture

#

## ✅ Features

- 🧭 Clean Architecture (Domain, Application, Infrastructure, Presentation)
- 🛠 Dapper (Micro ORM)
- 🧪 API + DTO Layer
- ⚙️ Global Exception Handling Middleware
- 📦 Response Wrapping Middleware
- 🔥 Logging ด้วย Serilog (Console + File)
- 📄 Swagger UI

#

## 🗂️ Project Structure

```bash
ProductAPI/
├── Domain/           # Entities + Interfaces (ไม่พึ่ง DB หรือ Framework)
├── Application/      # DTOs + Business Logic
├── Infrastructure/   # Repositories (Dapper)
├── Controllers/      # WebAPI Controllers
├── Middleware/       # Global Error & Response Wrapping Middleware
├── Common/           # ApiResponse, CustomException
├── appsettings.json  # Configuration
├── Program.cs        # Main Entry
└── Logs/             # Log Files (via Serilog)
````

#

## 🚀 How to Run (Local Dev)

### 1. สร้างฐานข้อมูล SQL Server

```sql
CREATE DATABASE ProductDb;

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL
);
```

### 2. ปรับ `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ProductDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Restore & Run

```bash
dotnet restore
dotnet run --project ProductAPI.csproj
```

#

## 📘 Sample API Endpoints

| Method | Endpoint          | Description          |
| ------ | ----------------- | -------------------- |
| GET    | `/api/products`   | Get all products     |
| GET    | `/api/products/1` | Get product by ID    |
| POST   | `/api/products`   | Create new product   |
| PUT    | `/api/products`   | Update product       |
| DELETE | `/api/products/1` | Delete product by ID |

#
## 🧰 Logging

* ติดตั้ง `Serilog` (console + file)
* ดู log ได้ที่ `Logs/log-YYYYMMDD.txt`

#

## 🧱 Clean Architecture Summary

| Layer          | Responsibility                         |
| -------------- | -------------------------------------- |
| Domain         | Entity, Interface, ไม่มีการอิง DB      |
| Application    | DTOs, Business Logic                   |
| Infrastructure | Dapper, SQL, Repository Implementation |
| Presentation   | Controllers, Middleware                |

#

## ✨ Response Format (Wrapped)

```json
{
  "success": true,
  "message": null,
  "data": [
    {
      "id": 1,
      "name": "Laptop X1",
      "price": 1200,
      "stock": 10,
      "createdAt": "2025-05-15T15:30:00",
      "updatedAt": null
    }
  ]
}
```

#

## 📚 Next Steps

* ✅ เพิ่ม Unit Tests (xUnit / Moq)
* ✅ เพิ่ม AutoMapper / FluentValidation
* ✅ รองรับ Pagination, Search
* ✅ Deploy บน Docker / Azure App Service / AKS

#

## 👨‍💻 Author

> Template นี้สนับสนุน โดยทีมพัฒนา (Core System)
 
### ขอบคุณ Chat {Chatgpt, Gemini, Ddeepseek, Qwen, Copilot}
