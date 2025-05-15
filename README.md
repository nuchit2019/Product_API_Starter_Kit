# CRUD Product API Starter Kit
Product API Starter Kit: .NET 8 + Dapper + Clean Architecture

#
## C# WebAPI ระบบ CRUD Product ด้วย Dapper, Clean Architecture และ SOLID Principles

คู่มือนี้จะแนะนำขั้นตอนการสร้างโปรเจกต์ WebAPI สำหรับจัดการ(CRUD) ข้อมูลสินค้า(Product) โดยใช้ภาษา C#, Dapper(ORM ขนาดเล็กสำหรับ .NET), ฐานข้อมูล MSSQL LocalDB, และสถาปัตยกรรมแบบ Clean Architecture พร้อมทั้งเน้นหลักการ Clean Code และ SOLID Principles เพื่อให้โค้ดมีคุณภาพ อ่านง่าย และง่ายต่อการบำรุงรักษา เหมาะสำหรับโปรแกรมเมอร์มือใหม่ที่ต้องการเรียนรู้การสร้าง WebAPI ที่มีโครงสร้างที่ดี

### 1. ความรู้พื้นฐานและเครื่องมือที่ต้องใช้

#### 1.1 Clean Architecture คืออะไร?
Clean Architecture เป็นสถาปัตยกรรมการออกแบบซอฟต์แวร์ที่เน้นการแยกส่วนประกอบ (Separation of Concerns) ทำให้โค้ดเป็นอิสระจาก Frameworks, UI, และ Database มากที่สุด โดยมีหัวใจหลักคือ **Domain Layer** และ **Application Layer** ที่ไม่ขึ้นกับส่วนอื่นๆ ทำให้ง่ายต่อการทดสอบ (Testable), บำรุงรักษา (Maintainable), และเปลี่ยนแปลง (Flexible)

โครงสร้างหลักๆ ของ Clean Architecture ที่เราจะใช้:
* **Domain:** ประกอบด้วย Entities, Value Objects, และ Domain Logic ที่สำคัญที่สุดของระบบ
* **Application:** ประกอบด้วย Use Cases (Application Logic), Interfaces ของ Repositories และ Services อื่นๆ
* **Infrastructure:** ประกอบด้วยการ Implement Interfaces จาก Application Layer เช่น Repositories (การเชื่อมต่อ Database), Services ภายนอก
* **Presentation (WebAPI):** ประกอบด้วย Controllers, DTOs (Data Transfer Objects) สำหรับรับส่งข้อมูล, และการตั้งค่า API

![image](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)
https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

#### 1.2 SOLID Principles คืออะไร?
SOLID เป็นหลักการ 5 ข้อในการออกแบบ Object-Oriented Programming เพื่อให้โค้ดมีความยืดหยุ่น เข้าใจง่าย และบำรุงรักษาง่าย:
* **S - Single Responsibility Principle (SRP):** Class หนึ่งควรมีหน้าที่รับผิดชอบเพียงอย่างเดียว
* **O - Open/Closed Principle (OCP):** Software entities (classes, modules, functions, etc.) ควรจะเปิดสำหรับการขยาย (extension) แต่ปิดสำหรับการแก้ไข (modification)
* **L - Liskov Substitution Principle (LSP):** Objects ของ Superclass ควรจะสามารถแทนที่ด้วย Objects ของ Subclass ได้โดยไม่กระทบการทำงานของโปรแกรม
* **I - Interface Segregation Principle (ISP):** Client ไม่ควรถูกบังคับให้ implement interface ที่ไม่ได้ใช้งาน
* **D - Dependency Inversion Principle (DIP):** High-level modules ไม่ควรขึ้นกับ Low-level modules แต่ทั้งคู่ควรขึ้นกับ Abstractions (Interfaces)

![image](https://github.com/user-attachments/assets/4e4a02da-8d36-4503-8c7c-4e4665cd718a)


#### 1.3 Dapper คืออะไร?
Dapper เป็น Micro ORM (Object-Relational Mapper) สำหรับ .NET ที่มีความเร็วสูง ใช้งานง่าย และให้ความยืดหยุ่นในการเขียน SQL Query โดยตรง

![image](https://github.com/user-attachments/assets/9b9a7864-5b92-4450-b2a0-292bcea74210)
https://blog.byalex.dev/article/dapper-queries-synchronized-with-mssql-database-schema


#### 1.4 เครื่องมือที่ต้องใช้:
1.  **Visual Studio 2022** (หรือเวอร์ชันใหม่กว่า) พร้อม .NET SDK (แนะนำ .NET 6.0 หรือใหม่กว่า)
2.  **SQL Server Management Studio (SSMS)** หรือเครื่องมือจัดการฐานข้อมูล SQL Server อื่นๆ
3.  **SQL Server LocalDB** (มักจะติดตั้งมาพร้อมกับ Visual Studio)
4.  **Postman** หรือเครื่องมืออื่นสำหรับทดสอบ API


### 2. การสร้างโปรเจกต์ (Project Creation)

เราจะสร้าง Solution และแบ่งออกเป็น 4 Projects ตามหลัก Clean Architecture:

1.  **Core.Domain:** สำหรับ Entities และ Interfaces หลักของ Domain
2.  **Core.Application:** สำหรับ Business Logic, Use Cases, และ Interfaces ของ Repositories
3.  **Infrastructure.Persistence:** สำหรับการ Implement Repositories และการเชื่อมต่อ Database
4.  **Presentation.WebAPI:** สำหรับ API Controllers และการตั้งค่า Web API

#### ขั้นตอนการสร้างโปรเจกต์ใน Visual Studio:

1.  เปิด Visual Studio.
2.  เลือก **Create a new project**.
3.  ค้นหา "Blank Solution" และตั้งชื่อ Solution เช่น `ProductManagement` จากนั้นคลิก **Create**.
   ![image](https://github.com/user-attachments/assets/168560fd-ab89-49cc-b030-6f595a1e3339)


4.  **สร้าง Project Core.Domain:**
    * ใน Solution Explorer, คลิกขวาที่ Solution `ProductManagement` -> **Add** -> **New Project...**.
    * เลือก **Class Library**. คลิก **Next**.
    * ตั้งชื่อ Project: `ProductManagement.Core.Domain`.
    * เลือก Framework (เช่น .NET 8.0). คลิก **Create**.
    ![image](https://github.com/user-attachments/assets/16d07700-b3ec-419a-9ff5-cfe4524a9d93)


5.  **สร้าง Project Core.Application:**
    * คลิกขวาที่ Solution -> **Add** -> **New Project...**.
    * เลือก **Class Library**.
    * ตั้งชื่อ Project: `ProductManagement.Core.Application`.
    * เลือก Framework. คลิก **Create**.
    * เพิ่ม Reference ให้ `Core.Application` อ้างอิงไปยัง `Core.Domain`:
        * คลิกขวาที่ `Core.Application` -> **Add** -> **Project Reference...**.
        * เลือก `ProductManagement.Core.Domain` และคลิก **OK**.
          (ขั้นตอนตามรูป ข้อ4.สร้าง Project Core.Domain)

6.  **สร้าง Project Infrastructure.Persistence:**
    * คลิกขวาที่ Solution -> **Add** -> **New Project...**.
    * เลือก **Class Library**.
    * ตั้งชื่อ Project: `ProductManagement.Infrastructure.Persistence`.
    * เลือก Framework. คลิก **Create**.
    * เพิ่ม Reference ให้ `Infrastructure.Persistence` อ้างอิงไปยัง `Core.Application`:
        * คลิกขวาที่ `Infrastructure.Persistence` -> **Add** -> **Project Reference...**.
        * เลือก `ProductManagement.Core.Application` และคลิก **OK**.
          (ขั้นตอนตามรูป ข้อ4.สร้าง Project Core.Domain)

7.  **สร้าง Project Presentation.WebAPI:**
    * คลิกขวาที่ Solution -> **Add** -> **New Project...**.
    * เลือก **ASP.NET Core Web API**. คลิก **Next**.
    * ตั้งชื่อ Project: `ProductManagement.Presentation.WebAPI`.
    * เลือก Framework. (Ensure "Configure for HTTPS" is checked, "Enable Docker" is unchecked, "Use controllers (uncheck to use minimal APIs)" is checked). คลิก **Create**.
    [Image of Visual Studio Create ASP.NET Core Web API]
    * เพิ่ม Reference ให้ `Presentation.WebAPI` อ้างอิงไปยัง `Core.Application` และ `Infrastructure.Persistence`:
        * คลิกขวาที่ `Presentation.WebAPI` -> **Add** -> **Project Reference...**.
        * เลือก `ProductManagement.Core.Application` และ `ProductManagement.Infrastructure.Persistence` และคลิก **OK**.
           (ขั้นตอนตามรูป ข้อ4.สร้าง Project Core.Domain) แต่ Project Type เปลี่ยนเป็น `ASP.NET Core Web API`
          ![image](https://github.com/user-attachments/assets/c996c473-7703-4786-9dc3-28270261d3c3)


ตอนนี้คุณควรจะมีโครงสร้างโปรเจกต์ดังนี้:
```
ProductManagement (Solution)
├── ProductManagement.Core.Application
├── ProductManagement.Core.Domain
├── ProductManagement.Infrastructure.Persistence
└── ProductManagement.Presentation.WebAPI
```

### 3. การออกแบบและสร้าง Database (MSSQL LocalDB)

1.  เปิด **SQL Server Management Studio (SSMS)**.
2.  เชื่อมต่อกับ Server Name: `(localdb)\\mssqllocaldb`. (นี่คือ Default instance name ของ LocalDB)
    [Image of SSMS Connect to LocalDB]
3.  ใน Object Explorer, คลิกขวาที่ **Databases** -> **New Database...**.
4.  ตั้งชื่อ Database: `ProductDb`. คลิก **OK**.
    [Image of SSMS Create New Database]
5.  ขยาย `ProductDb`, คลิกขวาที่ **Tables** -> **New** -> **Table...**.
6.  ออกแบบตาราง `Products` ดังนี้:
    * `Id` (int, PK, Identity Specification: Yes)
    * `Name` (nvarchar(255), Not Null)
    * `Description` (nvarchar(MAX), Null)
    * `Price` (decimal(18,2), Not Null)
    * `Stock` (int, Not Null)
    * `CreatedAt` (datetime2, Not Null, Default value: `GETUTCDATE()`)
    * `UpdatedAt` (datetime2, Null)
    [Image of SSMS Table Designer for Products]
7.  กด Ctrl + S เพื่อ Save Table, ตั้งชื่อว่า `Products`.

หรือใช้ SQL Script สร้างตาราง:
```sql
USE ProductDb;
GO

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Stock INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);
GO
```
รัน Script นี้ใน New Query window ของ `ProductDb`.

### 4. พัฒนาแต่ละ Layer

#### 4.1 Layer: Core.Domain

โปรเจกต์นี้จะเก็บ Entities และ Interfaces หลักของ Domain.

1.  **สร้าง Entity `Product`:**
    * ในโปรเจกต์ `ProductManagement.Core.Domain`, ลบไฟล์ `Class1.cs` ที่ถูกสร้างขึ้นมาอัตโนมัติ.
    * คลิกขวาที่โปรเจกต์ `ProductManagement.Core.Domain` -> **Add** -> **Class...**.
    * ตั้งชื่อไฟล์ `Product.cs`.
    ```csharp
    // File: ProductManagement.Core.Domain/Product.cs
    namespace ProductManagement.Core.Domain
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty; // กำหนดค่าเริ่มต้นเพื่อหลีกเลี่ยง CS8618
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
    ```
    *หมายเหตุ:* การใช้ `string.Empty` และ `?` (nullable reference types) ช่วยในการจัดการ Nullability ใน C# 8.0+

#### 4.2 Layer: Core.Application

โปรเจกต์นี้จะเก็บ Business Logic, Use Cases, และ Interfaces ของ Repositories.

1.  **ติดตั้ง NuGet Package ที่จำเป็น:**
    * ไม่จำเป็นต้องติดตั้ง Package ใดๆ ใน `Core.Application` สำหรับตัวอย่างนี้โดยตรง แต่ถ้ามี Validation หรือ Mapping library ที่ต้องการใช้ใน Application Layer ก็สามารถเพิ่มได้

2.  **สร้าง Interface `IProductRepository`:**
    * ในโปรเจกต์ `ProductManagement.Core.Application`, สร้าง Folder ชื่อ `Contracts/Persistence`.
    * คลิกขวาที่ Folder `Persistence` -> **Add** -> **New Item...** -> **Interface**.
    * ตั้งชื่อไฟล์ `IProductRepository.cs`.
    ```csharp
    // File: ProductManagement.Core.Application/Contracts/Persistence/IProductRepository.cs
    using ProductManagement.Core.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace ProductManagement.Core.Application.Contracts.Persistence
    {
        public interface IProductRepository
        {
            Task<Product?> GetByIdAsync(int id); // สามารถ return null ได้ถ้าไม่พบ Product
            Task<IEnumerable<Product>> GetAllAsync();
            Task<Product> AddAsync(Product product);
            Task<bool> UpdateAsync(Product product);
            Task<bool> DeleteAsync(int id);
        }
    }
    ```
    *หลักการ SOLID:*
    * **Dependency Inversion Principle (DIP):** Application Layer กำหนด Interface (`IProductRepository`) และ Infrastructure Layer จะ Implement Interface นี้ ทำให้ Application Layer ไม่ขึ้นกับรายละเอียดการ Implement ของ Database.

3.  **สร้าง DTOs (Data Transfer Objects):**
    * ในโปรเจกต์ `ProductManagement.Core.Application`, สร้าง Folder ชื่อ `DTOs/Product`.
    * **`ProductDto.cs`**:
        ```csharp
        // File: ProductManagement.Core.Application/DTOs/Product/ProductDto.cs
        namespace ProductManagement.Core.Application.DTOs.Product
        {
            public class ProductDto
            {
                public int Id { get; set; }
                public string Name { get; set; } = string.Empty;
                public string? Description { get; set; }
                public decimal Price { get; set; }
                public int Stock { get; set; }
                public DateTime CreatedAt { get; set; }
                public DateTime? UpdatedAt { get; set; }
            }
        }
        ```
    * **`CreateProductDto.cs`**:
        ```csharp
        // File: ProductManagement.Core.Application/DTOs/Product/CreateProductDto.cs
        using System.ComponentModel.DataAnnotations; // สำหรับ Validation Attributes

        namespace ProductManagement.Core.Application.DTOs.Product
        {
            public class CreateProductDto
            {
                [Required(ErrorMessage = "Product name is required.")]
                [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
                public string Name { get; set; } = string.Empty;

                public string? Description { get; set; }

                [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]
                public decimal Price { get; set; }

                [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
                public int Stock { get; set; }
            }
        }
        ```
    * **`UpdateProductDto.cs`**:
        ```csharp
        // File: ProductManagement.Core.Application/DTOs/Product/UpdateProductDto.cs
        using System.ComponentModel.DataAnnotations;

        namespace ProductManagement.Core.Application.DTOs.Product
        {
            public class UpdateProductDto
            {
                [Required(ErrorMessage = "Product ID is required for update.")]
                public int Id { get; set; } // จำเป็นต้องมี Id เพื่อระบุ Product ที่จะ Update

                [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
                public string? Name { get; set; } // ทำให้เป็น nullable ถ้าไม่ต้องการบังคับให้ update ทุก field

                public string? Description { get; set; }

                [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]
                public decimal? Price { get; set; }

                [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
                public int? Stock { get; set; }
            }
        }
        ```
    *หมายเหตุ:* `System.ComponentModel.DataAnnotations` ใช้สำหรับ Validation เบื้องต้น ซึ่ง WebAPI Framework จะตรวจสอบให้โดยอัตโนมัติเมื่อรับข้อมูลเข้ามาที่ Controller.

4.  **สร้าง Interface `IProductService`:**
    * ในโปรเจกต์ `ProductManagement.Core.Application`, สร้าง Folder ชื่อ `Contracts/Services`.
    * คลิกขวาที่ Folder `Services` -> **Add** -> **New Item...** -> **Interface**.
    * ตั้งชื่อไฟล์ `IProductService.cs`.
    ```csharp
    // File: ProductManagement.Core.Application/Contracts/Services/IProductService.cs
    using ProductManagement.Core.Application.DTOs.Product;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace ProductManagement.Core.Application.Contracts.Services
    {
        public interface IProductService
        {
            Task<ProductDto?> GetProductByIdAsync(int id);
            Task<IEnumerable<ProductDto>> GetAllProductsAsync();
            Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
            Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto);
            Task<bool> DeleteProductAsync(int id);
        }
    }
    ```

5.  **สร้าง Implementation `ProductService`:**
    * ในโปรเจกต์ `ProductManagement.Core.Application`, สร้าง Folder ชื่อ `Features/Products/Services` (หรือ `Services` ก็ได้ถ้าโครงสร้างไม่ซับซ้อนมาก).
    * คลิกขวาที่ Folder `Services` -> **Add** -> **Class...**.
    * ตั้งชื่อไฟล์ `ProductService.cs`.
    ```csharp
    // File: ProductManagement.Core.Application/Features/Products/Services/ProductService.cs
    using ProductManagement.Core.Application.Contracts.Persistence;
    using ProductManagement.Core.Application.Contracts.Services;
    using ProductManagement.Core.Application.DTOs.Product;
    using ProductManagement.Core.Domain; // ต้อง using Domain entity
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System; // สำหรับ DateTime.UtcNow

    namespace ProductManagement.Core.Application.Features.Products.Services
    {
        public class ProductService : IProductService
        {
            private readonly IProductRepository _productRepository;
            // อาจจะมี ILogger, IMapper (AutoMapper) หรือ Services อื่นๆ ถูก inject เข้ามาได้

            public ProductService(IProductRepository productRepository)
            {
                _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            }

            public async Task<ProductDto?> GetProductByIdAsync(int id)
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) return null;

                // Manual Mapping (หรือใช้ AutoMapper)
                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt
                };
            }

            public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
            {
                var products = await _productRepository.GetAllAsync();
                var productDtos = new List<ProductDto>();
                foreach (var product in products)
                {
                    productDtos.Add(new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Stock,
                        CreatedAt = product.CreatedAt,
                        UpdatedAt = product.UpdatedAt
                    });
                }
                return productDtos;
            }

            public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
            {
                // Validation เพิ่มเติมสามารถทำได้ที่นี่ (Business Logic)
                // เช่น ตรวจสอบว่าชื่อ Product ซ้ำหรือไม่ (ถ้าต้องการ)

                var product = new Product
                {
                    Name = createProductDto.Name,
                    Description = createProductDto.Description,
                    Price = createProductDto.Price,
                    Stock = createProductDto.Stock,
                    CreatedAt = DateTime.UtcNow // ใช้ UTC เพื่อความเป็นสากล
                };

                var newProduct = await _productRepository.AddAsync(product);

                return new ProductDto // คืนค่าเป็น DTO
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Stock = newProduct.Stock,
                    CreatedAt = newProduct.CreatedAt
                };
            }

            public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto)
            {
                var existingProduct = await _productRepository.GetByIdAsync(updateProductDto.Id);
                if (existingProduct == null)
                {
                    // หรือจะ throw new NotFoundException("Product not found"); ก็ได้
                    return false;
                }

                // Update fields if new value is provided
                existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
                existingProduct.Description = updateProductDto.Description ?? existingProduct.Description;
                existingProduct.Price = updateProductDto.Price ?? existingProduct.Price;
                existingProduct.Stock = updateProductDto.Stock ?? existingProduct.Stock;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                return await _productRepository.UpdateAsync(existingProduct);
            }

            public async Task<bool> DeleteProductAsync(int id)
            {
                var productToDelete = await _productRepository.GetByIdAsync(id);
                if (productToDelete == null)
                {
                    return false; // ไม่พบ Product ที่จะลบ
                }
                return await _productRepository.DeleteAsync(id);
            }
        }
    }
    ```
    *หลักการ SOLID:*
    * **Single Responsibility Principle (SRP):** `ProductService` รับผิดชอบเฉพาะ Business Logic ที่เกี่ยวกับ Product และการประสานงานกับ Repository.
    * **Open/Closed Principle (OCP):** ถ้าต้องการเพิ่ม Feature ใหม่ๆ เช่น การคำนวณส่วนลด ก็สามารถทำได้โดยการสร้าง Method ใหม่ หรือ Service ใหม่ โดยไม่กระทบ Method เดิม.

#### 4.3 Layer: Infrastructure.Persistence

โปรเจกต์นี้จะ Implement `IProductRepository` โดยใช้ Dapper และเชื่อมต่อกับ MSSQL.

1.  **ติดตั้ง NuGet Packages ที่จำเป็น:**
    * คลิกขวาที่โปรเจกต์ `ProductManagement.Infrastructure.Persistence` -> **Manage NuGet Packages...**.
    * ค้นหาและติดตั้ง:
        * `Dapper` (โดย DapperLib)
        * `Microsoft.Data.SqlClient` (สำหรับเชื่อมต่อ SQL Server)
        * `Microsoft.Extensions.Configuration.Abstractions` (ถ้าต้องการอ่าน Connection String จาก Configuration)
        * `Microsoft.Extensions.Configuration.Binder` (ถ้าต้องการอ่าน Connection String จาก Configuration)

2.  **สร้าง Implementation `ProductRepository`:**
    * ในโปรเจกต์ `ProductManagement.Infrastructure.Persistence`, สร้าง Folder ชื่อ `Repositories`.
    * คลิกขวาที่ Folder `Repositories` -> **Add** -> **Class...**.
    * ตั้งชื่อไฟล์ `ProductRepository.cs`.
    ```csharp
    // File: ProductManagement.Infrastructure.Persistence/Repositories/ProductRepository.cs
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration; // สำหรับ IConfiguration
    using ProductManagement.Core.Application.Contracts.Persistence;
    using ProductManagement.Core.Domain;
    using System.Collections.Generic;
    using System.Data; // สำหรับ IDbConnection
    using System.Threading.Tasks;
    using System; // สำหรับ DateTime.UtcNow

    namespace ProductManagement.Infrastructure.Persistence.Repositories
    {
        public class ProductRepository : IProductRepository
        {
            private readonly string _connectionString;

            // Constructor รับ IConfiguration เพื่ออ่าน Connection String จาก appsettings.json
            public ProductRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
            }

            // Helper method เพื่อสร้าง IDbConnection
            private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

            public async Task<Product?> GetByIdAsync(int id)
            {
                const string sql = "SELECT * FROM Products WHERE Id = @Id";
                using var connection = CreateConnection();
                return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                const string sql = "SELECT * FROM Products ORDER BY Name";
                using var connection = CreateConnection();
                return await connection.QueryAsync<Product>(sql);
            }

            public async Task<Product> AddAsync(Product product)
            {
                // CreatedAt ถูก set ใน Service Layer แล้ว หรือจะ set ที่นี่ก็ได้ถ้า Database ไม่ได้ set default
                // product.CreatedAt = DateTime.UtcNow; // ถ้าไม่ได้ set default ใน DB

                // SQL Server จะ generate Id ให้เอง (IDENTITY)
                // เราจะคืนค่า Product ที่มี Id ที่ถูก generate แล้ว
                const string sql = @"
                    INSERT INTO Products (Name, Description, Price, Stock, CreatedAt)
                    VALUES (@Name, @Description, @Price, @Stock, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)"; // SCOPE_IDENTITY() ใช้สำหรับดึง ID ล่าสุดที่ถูก generate ใน session ปัจจุบัน

                using var connection = CreateConnection();
                var newId = await connection.QuerySingleAsync<int>(sql, product);
                product.Id = newId; // กำหนด Id ให้กับ object product ที่รับเข้ามา
                return product;
            }

            public async Task<bool> UpdateAsync(Product product)
            {
                product.UpdatedAt = DateTime.UtcNow; // อัปเดตเวลาล่าสุด
                const string sql = @"
                    UPDATE Products
                    SET Name = @Name,
                        Description = @Description,
                        Price = @Price,
                        Stock = @Stock,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
                using var connection = CreateConnection();
                var affectedRows = await connection.ExecuteAsync(sql, product);
                return affectedRows > 0; // คืนค่า true ถ้ามีการ update เกิดขึ้น (มี row ที่ได้รับผลกระทบ)
            }

            public async Task<bool> DeleteAsync(int id)
            {
                const string sql = "DELETE FROM Products WHERE Id = @Id";
                using var connection = CreateConnection();
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }
    }
    ```
    *หลักการ SOLID:*
    * **Single Responsibility Principle (SRP):** `ProductRepository` รับผิดชอบเฉพาะการเข้าถึงข้อมูล Product ใน Database.
    * **Dependency Inversion Principle (DIP):** Implement `IProductRepository` ที่กำหนดโดย Application Layer.

3.  **ตั้งค่า Connection String:**
    * ในโปรเจกต์ `ProductManagement.Presentation.WebAPI` (หรือถ้าสร้าง `Infrastructure.Persistence` เป็น Console App เพื่อทดสอบ ก็ใน `appsettings.json` ของโปรเจกต์นั้น).
    * เปิดไฟล์ `appsettings.json` (ถ้าไม่มีให้สร้างขึ้นมา).
    * เพิ่ม Connection String:
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }
    ```
    *ตรวจสอบให้แน่ใจว่าชื่อ Server และ Database ถูกต้องตามที่สร้างไว้.*

#### 4.4 Layer: Presentation.WebAPI

โปรเจกต์นี้จะจัดการ HTTP Requests, Responses, และการตั้งค่า API.

1.  **ติดตั้ง NuGet Packages ที่จำเป็น (ถ้ายังไม่มี):**
    * `Swashbuckle.AspNetCore` (สำหรับ Swagger/OpenAPI UI) มักจะถูกติดตั้งมากับ Template Web API.

2.  **ตั้งค่า Dependency Injection (DI):**
    * เปิดไฟล์ `Program.cs` ในโปรเจกต์ `ProductManagement.Presentation.WebAPI`.
    * แก้ไขเพื่อลงทะเบียน Services และ Repositories.
    ```csharp
    // File: ProductManagement.Presentation.WebAPI/Program.cs
    using ProductManagement.Core.Application.Contracts.Persistence;
    using ProductManagement.Core.Application.Contracts.Services;
    using ProductManagement.Core.Application.Features.Products.Services; // Namespace ของ ProductService
    using ProductManagement.Infrastructure.Persistence.Repositories;    // Namespace ของ ProductRepository

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // 1. ลงทะเบียน IConfiguration เพื่อให้ Repository สามารถอ่าน ConnectionString ได้
    // builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // .NET 6 builder.Configuration มีให้ใช้อยู่แล้ว

    // 2. ลงทะเบียน Repository และ Service
    // ใช้ AddScoped สำหรับ Services และ Repositories ที่เกี่ยวข้องกับ DB Context หรือ HTTP Request Lifecycle
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at [https://aka.ms/aspnetcore/swashbuckle](https://aka.ms/aspnetcore/swashbuckle)
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

    app.MapControllers();

    app.Run();
    ```

3.  **สร้าง `ProductsController`:**
    * ในโปรเจกต์ `ProductManagement.Presentation.WebAPI`, ใน Folder `Controllers`, ลบ `WeatherForecastController.cs` (ถ้ามี).
    * คลิกขวาที่ Folder `Controllers` -> **Add** -> **Controller...**.
    * เลือก **API Controller - Empty**. คลิก **Add**.
    * ตั้งชื่อ `ProductsController.cs`.
    ```csharp
    // File: ProductManagement.Presentation.WebAPI/Controllers/ProductsController.cs
    using Microsoft.AspNetCore.Mvc;
    using ProductManagement.Core.Application.Contracts.Services;
    using ProductManagement.Core.Application.DTOs.Product;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging; // สำหรับ ILogger

    namespace ProductManagement.Presentation.WebAPI.Controllers
    {
        [Route("api/[controller]")] // URL: /api/products
        [ApiController]
        public class ProductsController : ControllerBase
        {
            private readonly IProductService _productService;
            private readonly ILogger<ProductsController> _logger; // Inject ILogger

            public ProductsController(IProductService productService, ILogger<ProductsController> logger)
            {
                _productService = productService ?? throw new System.ArgumentNullException(nameof(productService));
                _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }

            // GET: api/Products
            [HttpGet]
            [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
            {
                _logger.LogInformation("Attempting to retrieve all products.");
                var products = await _productService.GetAllProductsAsync();
                _logger.LogInformation($"Successfully retrieved {((List<ProductDto>)products).Count} products.");
                return Ok(products);
            }

            // GET: api/Products/5
            [HttpGet("{id}")]
            [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ProductDto>> GetProductById(int id)
            {
                _logger.LogInformation($"Attempting to retrieve product with ID: {id}.");
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning($"Product with ID: {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }
                _logger.LogInformation($"Successfully retrieved product with ID: {id}.");
                return Ok(product);
            }

            // POST: api/Products
            [HttpPost]
            [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
            {
                // ModelState.IsValid จะถูกตรวจสอบโดย [ApiController] attribute โดยอัตโนมัติ
                // ถ้าไม่ valid จะ return 400 Bad Request พร้อมรายละเอียด Error
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("CreateProduct request failed due to invalid model state.");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"Attempting to create a new product with name: {createProductDto.Name}.");
                try
                {
                    var newProduct = await _productService.CreateProductAsync(createProductDto);
                    _logger.LogInformation($"Successfully created product with ID: {newProduct.Id}.");
                    // คืนค่า 201 Created พร้อม Location header และ Object ที่สร้างขึ้น
                    return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
                }
                catch (System.Exception ex) // ควรจะ Catch Exception ที่เฉพาะเจาะจงกว่านี้
                {
                    _logger.LogError(ex, $"Error occurred while creating product with name: {createProductDto.Name}.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
                }
            }

            // PUT: api/Products/5
            [HttpPut("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
            {
                if (id != updateProductDto.Id)
                {
                    _logger.LogWarning($"Mismatched ID in URL ({id}) and body ({updateProductDto.Id}) for update request.");
                    return BadRequest("Mismatched product ID in URL and request body.");
                }

                if (!ModelState.IsValid)
                {
                     _logger.LogWarning($"UpdateProduct request for ID: {id} failed due to invalid model state.");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"Attempting to update product with ID: {id}.");
                var success = await _productService.UpdateProductAsync(updateProductDto);

                if (!success)
                {
                    _logger.LogWarning($"Product with ID: {id} not found for update.");
                    return NotFound($"Product with ID {id} not found for update.");
                }
                _logger.LogInformation($"Successfully updated product with ID: {id}.");
                return NoContent(); // 204 No Content คือ Standard response สำหรับ successful PUT ที่ไม่มี content คืน
            }

            // DELETE: api/Products/5
            [HttpDelete("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                _logger.LogInformation($"Attempting to delete product with ID: {id}.");
                var success = await _productService.DeleteProductAsync(id);

                if (!success)
                {
                    _logger.LogWarning($"Product with ID: {id} not found for deletion.");
                    return NotFound($"Product with ID {id} not found for deletion.");
                }
                _logger.LogInformation($"Successfully deleted product with ID: {id}.");
                return NoContent();
            }
        }
    }
    ```
    *หลักการ Clean Code:*
    * **Clear Naming:** ชื่อ Method และ Variable สื่อความหมาย.
    * **Small Methods:** แต่ละ Action Method มีหน้าที่เดียว.
    * **Comments:** อธิบายส่วนที่ซับซ้อน หรือการตัดสินใจในการออกแบบ.
    * **Logging:** เพิ่ม Log เพื่อติดตามการทำงานและช่วยในการ Debug.
    *หลักการ SOLID:*
    * **Single Responsibility Principle (SRP):** Controller รับผิดชอบการจัดการ HTTP Request/Response และส่งต่อให้ Service Layer. ไม่ควรมี Business Logic ใน Controller.

### 5. การทดสอบ API (Testing with Postman)

1.  **Run the WebAPI Project:**
    * ใน Visual Studio, เลือก `ProductManagement.Presentation.WebAPI` เป็น Startup Project.
    * กด F5 หรือปุ่ม Start Debugging.
    * Browser จะเปิดขึ้นมา (มักจะเป็นหน้า Swagger UI ที่ `https://localhost:<port>/swagger/index.html`).
    [Image of Swagger UI for Product API]

2.  **ใช้ Postman (หรือ Swagger UI) ทดสอบ Endpoints:**

    * **Create Product (POST `/api/products`):**
        * Method: `POST`
        * URL: `https://localhost:<your_port>/api/products` (ดู Port จากตอน Run)
        * Body (raw, JSON):
            ```json
            {
              "name": "Laptop Pro 15",
              "description": "High-performance laptop for professionals",
              "price": 1200.99,
              "stock": 50
            }
            ```
        * Expected Response: `201 Created` พร้อมข้อมูล Product ที่สร้างขึ้น.

    * **Get All Products (GET `/api/products`):**
        * Method: `GET`
        * URL: `https://localhost:<your_port>/api/products`
        * Expected Response: `200 OK` พร้อม List ของ Products.

    * **Get Product by ID (GET `/api/products/{id}`):**
        * Method: `GET`
        * URL: `https://localhost:<your_port>/api/products/1` (แทน `1` ด้วย ID ที่มีอยู่จริง)
        * Expected Response: `200 OK` พร้อมข้อมูล Product หรือ `404 Not Found`.

    * **Update Product (PUT `/api/products/{id}`):**
        * Method: `PUT`
        * URL: `https://localhost:<your_port>/api/products/1`
        * Body (raw, JSON):
            ```json
            {
              "id": 1,
              "name": "Laptop Pro 15 (Updated)",
              "description": "Updated description",
              "price": 1250.00,
              "stock": 45
            }
            ```
        * Expected Response: `204 No Content` หรือ `404 Not Found` หรือ `400 Bad Request`.

    * **Delete Product (DELETE `/api/products/{id}`):**
        * Method: `DELETE`
        * URL: `https://localhost:<your_port>/api/products/1`
        * Expected Response: `204 No Content` หรือ `404 Not Found`.

### 6. สรุปและข้อเสนอแนะเพิ่มเติม

เราได้สร้าง WebAPI สำหรับ CRUD Product โดยใช้ C#, Dapper, MSSQL LocalDB, Clean Architecture, และ SOLID Principles. โครงสร้างนี้ช่วยให้:
* **Testability:** สามารถทดสอบ Application Layer และ Domain Layer ได้ง่ายโดยไม่ต้องพึ่ง Database หรือ UI.
* **Maintainability:** การแก้ไขหรือเพิ่มเติม Feature ในส่วนหนึ่งไม่กระทบส่วนอื่นมากนัก.
* **Flexibility:** สามารถเปลี่ยน Database (เช่น จาก MSSQL เป็น PostgreSQL) หรือ UI Framework ได้ง่ายขึ้น โดยกระทบ Core Logic น้อยที่สุด.

**ข้อเสนอแนะเพิ่มเติมสำหรับโปรแกรมเมอร์มือใหม่:**
* **Error Handling:** Implement Global Error Handling Middleware เพื่อจัดการ Exception อย่างเป็นระบบ.
* **Logging:** ใช้ Logging Framework ที่มีความสามารถสูงขึ้น เช่น Serilog, NLog และ Log ไปยัง File, Database, หรือ Centralized Logging System.
* **Mapping:** ใช้ AutoMapper หรือ Mapster เพื่อลด Boilerplate Code ในการแปลง Object (เช่น Entity <-> DTO).
* **Validation:** ใช้ FluentValidation สำหรับ Validation Rules ที่ซับซ้อนและอ่านง่ายขึ้น.
* **Async/Await:** ทำความเข้าใจการใช้งาน `async` และ `await` ให้ถูกต้องเพื่อประสิทธิภาพที่ดี.
* **Unit Testing & Integration Testing:** เขียน Test เพื่อให้มั่นใจว่าโค้ดทำงานถูกต้อง.
* **Security:** พิจารณาเรื่อง Authentication และ Authorization (เช่น JWT).
* **Configuration Management:** จัดการ Configuration ต่างๆ (เช่น Connection Strings, API Keys) อย่างปลอดภัย (เช่น User Secrets, Azure Key Vault).

หวังว่าคู่มือนี้จะเป็นประโยชน์และช่วยให้เข้าใจการสร้าง WebAPI ที่มีคุณภาพได้ดียิ่งขึ้นนะครับ!
