# คู่มือ... CRUD Product API Starter Kit
## **โครงสร้าง Project: ProductAPI** อิงหลัก **Clean Architecture**, **SOLID Principles** และแนวทาง **Clean Code**...:

Project นี้จะแนะนำขั้นตอนการสร้างโปรเจกต์ WebAPI สำหรับจัดการ(CRUD) ข้อมูลสินค้า(Product) โดยใช้ภาษา C#, Dapper(ORM ขนาดเล็กสำหรับ .NET), ฐานข้อมูล MSSQL LocalDB, และสถาปัตยกรรมแบบ Clean Architecture พร้อมทั้งเน้นหลักการ Clean Code และ SOLID Principles เพื่อให้โค้ดมีคุณภาพ อ่านง่าย และง่ายต่อการบำรุงรักษา เหมาะสำหรับโปรแกรมเมอร์มือใหม่ที่ต้องการเรียนรู้การสร้าง WebAPI ที่มีโครงสร้างที่ดี และนำไปสู่ การพัฒนา Microservice ต่อไป... 

---

## 1. ความรู้พื้นฐาน และเครื่องมือที่ใช้...

* 1.1 Clean Architecture คืออะไร?
* 1.2 SOLID Principles คืออะไร?
* 1.3 High Cohesion and Low Coupling คืออะไร?
* 1.4 record คืออะไร?
* 1.5 Dapper คืออะไร?
* 1.6 Serilog คืออะไร?

---

### 🔹 1.1 Clean Architecture คืออะไร?
Clean Architecture เป็นสถาปัตยกรรมการออกแบบซอฟต์แวร์ที่เน้นการแยกส่วนประกอบ (Separation of Concerns) ทำให้โค้ดเป็นอิสระจาก Frameworks, UI, และ Database มากที่สุด โดยมีหัวใจหลักคือ **Domain Layer** และ **Application Layer** ที่ไม่ขึ้นกับส่วนอื่นๆ ทำให้ง่ายต่อการทดสอบ (Testable), บำรุงรักษา (Maintainable), และเปลี่ยนแปลง (Flexible)

โครงสร้างหลักๆ ของ Clean Architecture ที่เราจะใช้:
* **Domain:** ประกอบด้วย Entities, Value Objects, และ Domain Logic ที่สำคัญที่สุดของระบบ
* **Application:** ประกอบด้วย Use Cases (Application Logic), Interfaces ของ Repositories และ Services อื่นๆ
* **Infrastructure:** ประกอบด้วยการ Implement Interfaces จาก Application Layer เช่น Repositories (การเชื่อมต่อ Database), Services ภายนอก
* **Presentation (WebAPI):** ประกอบด้วย Controllers, DTOs (Data Transfer Objects) สำหรับรับส่งข้อมูล, และการตั้งค่า API

![image](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)
https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

ภาพนี้คือ **`Clean Architecture`** ซึ่งเป็นแนวคิดในการออกแบบซอฟต์แวร์โดย Robert C. Martin (Uncle Bob) ที่เน้นความ **แยกส่วน (Separation of Concerns)** และการควบคุม **Dependency Flow** เพื่อให้โค้ดดูแลรักษาง่าย ทดสอบได้ง่าย และไม่ผูกติดกับ Framework หรือ Technology ใด ๆ

---

## 🧠 โครงสร้างวงกลม (Layers) ในภาพ

### 1. 🟡 **Entities** – *Enterprise Business Rules*

* คือ **แก่นกลางของระบบ** (Core Model/Domain)
* เก็บกฎเกณฑ์ทางธุรกิจ (Business Logic) ระดับองค์กร
* ไม่มีการอิงกับ Framework ใด ๆ
* ตัวอย่าง: `Product`, `Order`, `Customer`, `Policy`, พร้อม method เช่น `CalculateTotal()`

---

### 2. 🔴 **Use Cases** – *Application Business Rules*

* แสดง **การทำงานของระบบในระดับธุรกิจ**
* คือ Logic ที่ใช้จัดการกับการร้องขอจากผู้ใช้ เช่น "Create Order", "Calculate Quote"
* เรียกใช้งาน Entities และควบคุมการไหลของข้อมูลระหว่าง Layer
* เป็นหัวใจของ Business Process

---

### 3. 🟢 **Interface Adapters** – *Controllers, Presenters, Gateways*

* เป็น “ตัวแปลง” (Adapter) ระหว่างภายนอกกับ Use Case
* ทำหน้าที่แปลง:

  * Input → ให้เข้ากับ Use Case
  * Output → ให้แสดงผลในรูปแบบที่ View เข้าใจได้
* แบ่งเป็น:

  * **Controllers** (จาก Web/HTTP Request)
  * **Presenters** (ส่งผลลัพธ์ไปยัง View หรือ Response)
  * **Gateways** (แปลงข้อมูลจาก DB/External ให้เข้าใจ Use Case)

---

### 4. 🔵 **Frameworks & Drivers** – *UI, DB, Devices, Web*

* เป็นส่วนที่อยู่ **นอกสุด** และสามารถเปลี่ยนแปลงได้
* เช่น Web Framework (ASP.NET), Database (SQL/Oracle), Messaging (Kafka), External APIs ฯลฯ
* **ห้ามให้ Business Logic ผูกกับ Layer นี้**
* เปรียบเหมือน “ปลั๊กอิน” ที่สามารถถอดเปลี่ยนได้

---

## 🔁 Dependency Rule (กฎหลักของ Clean Architecture)

* **“โค้ดภายในห้ามรู้จักโค้ดภายนอก”**
* หรือพูดง่าย ๆ ว่า **การพึ่งพา (Dependency) ต้องไหลจากนอกเข้าข้างในไม่ได้**
* **ต้องชี้จากนอกเข้าใน (Outside → Inside)** เท่านั้น ไม่สามารถพึ่งพากลับจากภายในไปยังภายนอกได้
* Layer ด้านนอก *สามารถ* รู้จัก Layer ด้านในได้
  ✅ Interface สามารถกำหนดใน Use Case / Domain แล้วให้ Framework ภายนอก Implement

---

## 📊 Flow of Control (ขวาล่างของภาพ)

1. **Controller** รับ Input → ส่งให้ Use Case
2. **Use Case Interactor** ประมวลผล → เรียก Entity
3. **ผลลัพธ์** ถูกส่งผ่าน **Use Case Output Port** ไปยัง **Presenter**
4. **Presenter** แปลงผลลัพธ์ → ส่งไป UI (View/Response)

---

## 🔧 ตัวอย่างในโปรเจกต์ .NET WebAPI

| Layer                | โปรเจกต์/โฟลเดอร์          | เนื้อหา                                     |
| -------------------- | -------------------------- | ------------------------------------------- |
| Entities             | `Domain`                   | Class เช่น `Product`, `Policy`              |
| Use Cases            | `Application`              | Service/Handler ที่ทำงานกับ Business Logic  |
| Interface Adapters   | `WebAPI`, `Infrastructure` | Controllers, DTO, Repository Implementation |
| Frameworks & Drivers | ASP.NET, EF Core, Serilog  | ใช้เป็นเครื่องมือ แต่ไม่ผูกติดใน Use Case   |

---

## ✅ สรุปข้อดีของ Clean Architecture

* **ทดสอบง่าย** (Business Logic ไม่ผูกกับ DB หรือ Framework)
* **เปลี่ยนเทคโนโลยีง่าย** เช่นเปลี่ยนจาก SQL เป็น Mongo ได้ทันที
* **แยกส่วนความรับผิดชอบชัดเจน** (SRP)
* **รองรับการขยายระบบได้ดี**

---
 

#
### 1.2 SOLID Principles คืออะไร?

SOLID เป็นคำย่อ ของอักษร 5 ตัว ... และมันคือชุดของหลักการ 5 ข้อ ที่ช่วยให้เราเขียนโค้ด OOP ที่มีคุณภาพ โดยเน้นให้โค้ด แยกหน้าที่ชัดเจน, ยืดหยุ่น, บำรุงรักษาง่าย, และ รองรับการขยายในอนาคต

![image](https://github.com/user-attachments/assets/4e4a02da-8d36-4503-8c7c-4e4665cd718a)

| ย่อ   | เต็ม                     | ความหมายโดยย่อ                      |
| ----- | ------------------------------- | ----------------------------------- |
| **S** | Single Responsibility Principle (SRP) | หนึ่งคลาสมีหน้าที่เดียว             |
| **O** | Open/Closed Principle (SRP)           | แก้ระบบโดยไม่ต้องแก้โค้ดเดิม        |
| **L** | Liskov Substitution Principle (SRP)   | Subclass แทน Superclass ได้         |
| **I** | Interface Segregation Principle (SRP) | Interface แยกเฉพาะสิ่งที่จำเป็น     |
| **D** | Dependency Inversion Principle (SRP)  | พึ่ง abstraction, interface ไม่พึ่ง class จริง |

#
### 🔹 1.3 High Cohesion and Low Coupling คืออะไร?

* **High Cohesion (การรวมกลุ่มของความรับผิดชอบที่เกี่ยวข้องกันไว้ด้วยกัน)** หมายถึง โมดูล (Class/Service/Function) ควรมีความรับผิดชอบที่ เกี่ยวข้องกันอย่างชัดเจน และ ทำหน้าที่เฉพาะทาง ไม่กระจายความรับผิดชอบไปยังหลายเรื่อง
* **Low Coupling (การพึ่งพากันน้อยที่สุดระหว่างโมดูล)** หมายถึง โมดูลควร พึ่งพาโมดูลอื่นให้น้อยที่สุด และ มีอิสระต่อกัน เพื่อให้สามารถเปลี่ยนแปลงหรือทดสอบได้โดยไม่กระทบกันมาก
![image](https://github.com/user-attachments/assets/f92ebcc4-db33-48f2-9dd2-81eca7a5b374)

จากภาพ...เป็นการเปรียบเทียบระหว่าง **Low Cohesion-High Coupling** กับ **High Cohesion-Low Coupling** ซึ่งเป็นแนวคิดพื้นฐานที่สำคัญใน **การออกแบบซอฟต์แวร์**
#
### 🔴 ฝั่งซ้าย: Low Cohesion and High Coupling : เกาะกลุ่มต่ำ-เชื่อมโยงสูง

**ลักษณะ:**

* ภายในแต่ละโมดูล (กล่อง) มี **หลายหน้าที่ผสมกัน** เช่น มีวงกลม, สี่เหลี่ยม, สามเหลี่ยมในกล่องเดียว
* โมดูลต่าง ๆ **เชื่อมโยงกันเยอะมาก** (เส้นโยงหลายทิศทาง)

**ปัญหาที่เกิดขึ้น:**

1. **Low Cohesion (การเกาะกลุ่มต่ำ):**

   * หน้าที่ภายในโมดูลไม่สัมพันธ์กัน
   * ดูแลและเปลี่ยนแปลงโค้ดยาก เพราะแต่ละโมดูลมีหลายหน้าที่
2. **High Coupling (การเชื่อมโยงสูง):**

   * โมดูลแต่ละตัวพึ่งพากันมาก
   * เปลี่ยนแปลงโมดูลหนึ่ง อาจทำให้โมดูลอื่นพังตาม

#
 
### 🟢 ฝั่งขวา: High Cohesion and Low Coupling : เกาะกลุ่มสูง-เชื่อมโยงต่ำ

**ลักษณะ:**

* แต่ละโมดูล (กล่อง) มีองค์ประกอบที่ **คล้ายคลึงและมีหน้าที่เฉพาะ** เช่น กล่องหนึ่งมีแต่วงกลม, อีกกล่องมีแต่สามเหลี่ยม
* เส้นการเชื่อมโยงระหว่างโมดูลมีน้อยมาก และเชื่อมเฉพาะที่จำเป็น

**ข้อดี:**

1. **High Cohesion (การเกาะกลุ่มสูง):**

   * ทุกหน้าที่ในโมดูลมีเป้าหมายเดียวกัน
   * แก้ไขง่าย ดูแลง่าย เพราะมีความรับผิดชอบชัดเจน
2. **Low Coupling (การเชื่อมโยงต่ำ):**

   * โมดูลอิสระต่อกัน เปลี่ยนโมดูลใดโมดูลหนึ่ง ไม่กระทบตัวอื่น
   * ทำให้ **Unit Test, Refactor, และ Scaling ระบบง่ายขึ้น**

#
### 🧠 สรุป:

| ประเภท                         | ลักษณะ                           | ผลกระทบ                            |
| ------------------------------ | -------------------------------- | ---------------------------------- |
| ❌ Low Cohesion + High Coupling | หน้าที่ปนกัน / โมดูลพึ่งกันมาก   | แก้ไขยาก, เปราะบาง                 |
| ✅ High Cohesion + Low Coupling | หน้าที่เฉพาะเจาะจง / อิสระต่อกัน | ดูแลง่าย, เปลี่ยนแปลงง่าย, ขยายได้ |

#

... **ภาพฝั่งขวาคือเป้าหมาย**ที่เราควรมุ่งไป...
#

### ไปดูตัวตัวอย่าง **ระบบ CRUD Product API** :

#

## 🔴 ตัวอย่างที่ไม่ดี (Low Cohesion, High Coupling) เกาะกลุ่มต่ำ-เชื่อมโยงสูง

```csharp
// ❌ ProductController.cs (มีหลายความรับผิดชอบรวมกัน)
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly SqlConnection _connection;

    public ProductController()
    {
        _connection = new SqlConnection("conn_string");
    }

     [HttpGet]
    public IActionResult GetProducts()
    {
        var products = new List<Product>();
        var sql = "SELECT Id, Name, Price FROM Products";
        var cmd = new SqlCommand(sql, _connection);
        var reader = cmd.ExecuteReader(); 

        while (reader.Read())
        {
            var product = new Product
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Price = (decimal)reader["Price"]
            };
            products.Add(product);
        }
        reader.Close();
        return Ok(products); // ❌ รวม Logic, DB, Response ใน method เดียว
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    { 
        var sql = $"INSERT INTO Products (Name, Price) VALUES ('{product.Name}', {product.Price})";
        var cmd = new SqlCommand(sql, _connection);
        cmd.ExecuteNonQuery(); 

        return Ok(new { message = "Product created successfully" });
    }
}
```

### 🔴 ปัญหาของโค้ดนี้ 

* ❌ **Low Cohesion**: Controller ทำทั้ง Connection, SQL, Mapping → ไม่แยกความรับผิดชอบ
* ❌ **High Coupling**: Controller ผูกแน่นกับ DB และ SQL โดยตรง
* ❌ ทำให้ Test, Reuse, Maintenance ยาก

#

## 🟢 ตัวอย่างที่ดี (High Cohesion, Low Coupling) เกาะกลุ่มสูง-เชื่อมโยงต่ำ

### ✅ 1. **High Cohesion:**

Controller ทำหน้าที่เฉพาะเจาะจง:
**รับ Request → เรียก Service → Return Response**

* ❌ ไม่ทำ logic
* ❌ ไม่เขียน SQL
* ❌ ไม่เชื่อม DB
* ✅ โฟกัสแค่การ "Routing" และ "Response"

### ✅ 2. **Low Coupling:**

ผูกกับแค่ `IProductService` (Interface) ไม่รู้ว่าเบื้องหลังทำยังไง

* 🔁 Service จริง, mock, หรือ test double สามารถ Inject เข้าไปได้หมด
* 🔄 เปลี่ยน Database, Business Logic หรือ Storage ได้ โดยไม่ต้องแก้ Controller
* 
```csharp
// ✅ ProductController.cs – รับผิดชอบแค่... รับ Request → เรียก Service → Return Response
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var newProduct = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = newProduct.Id }, newProduct);
    }
}
```


```csharp
// ✅ ProductService.cs – ทำหน้าที่เฉพาะเรื่อง Business Logic
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        var id = await _repository.InsertAsync(product);
        product.Id = id;
        return new ProductDto(product);
    }
}
```
## ✅ จุดเด่นของ `ProductService.cs`

✅ High Cohesion	คลาสนี้มีหน้าที่รับผิดชอบ เฉพาะเรื่อง Business Logic
✅ Low Coupling	ใช้ IProductRepository ทำให้ไม่รู้ว่าการเข้าถึงข้อมูลใช้ Dapper, EF, หรืออะไร – เปลี่ยนได้ง่าย
✅ Single Responsibility	ไม่สนใจการเชื่อมต่อฐานข้อมูล หรือการ mapping HTTP
✅ Testable	Inject IProductRepository ได้ ทำให้เขียน Unit Test ง่าย
✅ Reusable	Logic สามารถถูกเรียกใช้จาก API หรือ Background Job ได้


```csharp
// ✅ ProductRepository.cs – จัดการ DB อย่างเดียว
public class ProductRepository : IProductRepository
{
    private readonly IDbConnection _db;

    public ProductRepository(IConfiguration config)
    {
        _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var sql = "SELECT * FROM Products";
        return await _db.QueryAsync<Product>(sql);
    }

    public async Task<int> InsertAsync(Product product)
    {
        var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _db.ExecuteScalarAsync<int>(sql, product);
    }
}
```

#

## ✅ จุดเด่นของแนวทางนี้

| หลักการ                | การนำไปใช้                                                                 |
| ---------------------- | -------------------------------------------------------------------------- |
| **High Cohesion**      | Controller → Routing / Service → Business Logic / Repo → DB Access         |
| **Low Coupling**       | Layer ต่าง ๆ ใช้ Interface (IProductService, IProductRepository) เชื่อมกัน |
| **Test ได้ง่าย**       | แต่ละ Layer test แยกได้ด้วย Mock                                           |
| **เปลี่ยน DB ได้ง่าย** | ถ้าเปลี่ยนจาก MSSQL → PostgreSQL เปลี่ยนแค่ Repository                     |
 
#
### 🔹 1.4 record  คืออะไร?
`record` คือ **ชนิดข้อมูล (data type)** แบบใหม่ใน C# ที่ถูกเพิ่มเข้ามาตั้งแต่ **C# 9.0** ซึ่งออกแบบมาเพื่อใช้กับ **ข้อมูลที่เน้นการเก็บค่า (data-centric)** มากกว่า **พฤติกรรม (behavior)**

#### 🧾 ความหมายของ `record`

> `record` คือชนิดข้อมูลที่ถูกออกแบบมาให้เก็บข้อมูลแบบ Immutable โดย Default และใช้ **Value-Based Equality** แทน **Reference-Based Equality** เหมือน class ปกติ

#### 🔍 จุดเด่นของ `record`

| จุดเด่น                                | รายละเอียด                                                    |
| -------------------------------------- | ------------------------------------------------------------- |
| ✅ **Value Equality**                   | เมื่อเทียบ object จะเปรียบเทียบค่าทุก property ไม่ใช่ address |
| ✅ **Immutable by default**             | ใช้ `init` แทน `set`, ป้องกันการเปลี่ยนค่าหลังสร้าง           |
| ✅ **Concise Syntax**                   | เขียนสั้นกว่า class โดยเฉพาะแบบ positional                    |
| ✅ **Built-in Copy**                    | ใช้ `with` expression เพื่อ copy แล้วเปลี่ยนบางค่า            |
| ✅ **เหมาะกับ DTO / ViewModel / Event** | เพราะมี behavior น้อย และต้องการความชัดเจนด้านข้อมูล          |
 
#### 🧠 เหมาะกับอะไร?

| ใช้ `record` เมื่อ...                        | เหตุผล                            |
| -------------------------------------------- | --------------------------------- |
| ✅ ต้องการ Immutable Object                   | ป้องกันค่าถูกแก้ไขโดยไม่ได้ตั้งใจ |
| ✅ สร้าง DTO / ViewModel                      | ส่งข้อมูลแบบไม่มี logic ภายใน     |
| ✅ ต้องการ Copy แล้วเปลี่ยนค่าแค่บางส่วน      | ใช้ `with` expression             |
| ✅ ต้องการเปรียบเทียบด้วยค่า (Value Equality) | เช่น ใน Unit Test                 |

#
#### 📌 สรุปสั้น ๆ

> `record` = **"class ที่เน้นเก็บข้อมูล"**
> ✅ Immutable, ✅ Value Equality, ✅ Syntax กระชับ
> เหมาะกับ DTO, Response, Command, Query ใน Clean Architecture

#
### 🔹 1.5 Dapper คืออะไร?
Dapper เป็น Micro ORM (Object-Relational Mapper) สำหรับ .NET ที่มีความเร็วสูง ใช้งานง่าย และให้ความยืดหยุ่นในการเขียน SQL Query โดยตรง ถ้า Query คุณยาว 8 เมตร ... จงใช้มันเถอะ...

![image](https://github.com/user-attachments/assets/9b9a7864-5b92-4450-b2a0-292bcea74210)
https://blog.byalex.dev/article/dapper-queries-synchronized-with-mssql-database-schema
 
---

### 🔹 1.6 Serilog คืออะไร?

**Serilog** คือ **logging library** (ไลบรารีสำหรับบันทึก log) ที่ถูกออกแบบมาให้ใช้งานง่ายและทันสมัยใน .NET (C#) โดยมีจุดเด่นคือ:

* ✅ รองรับ **structured logging** (การ log แบบมีโครงสร้าง เช่น JSON)
* ✅ ทำงานร่วมกับ **Microsoft.Extensions.Logging** ได้ (ตัว logging built-in ของ .NET Core)
* ✅ สามารถส่ง log ไปยังหลายที่ได้ (เช่น Console, File, Seq, Elasticsearch, Application Insights, Datadog, Grafana Loki ฯลฯ) ผ่าน **Sinks**
* ✅ ตั้งค่าและใช้งานง่ายมาก ทั้งในโค้ดและผ่าน `appsettings.json`

#

### 🔹 Serilog เริ่มพัฒนาเมื่อไหร่?

Serilog ถูกพัฒนาโดย **Nicholas Blumhardt**
🔸 เปิดตัวครั้งแรกบน GitHub ในช่วง **ปี 2013**
🔸 ปัจจุบันยังมีการพัฒนาอย่างต่อเนื่อง และเป็นหนึ่งในไลบรารียอดนิยมด้าน Logging บน .NET

#

### 🔹 เหมาะกับแอปพลิเคชันแบบไหน?

Serilog เหมาะกับแอปที่ต้องการ **Logging ที่ดี อ่านง่าย วิเคราะห์ง่าย และขยายได้ดี** โดยเฉพาะ:

#### ✅ เหมาะกับ:

| ประเภทแอป                                                 | เหตุผลที่เหมาะ                                      |
| --------------------------------------------------------- | --------------------------------------------------- |
| 🌐 **Web API / Web Apps (.NET Core)**                     | มี Middleware, Request/Response Logging, Trace ได้  |
| 🛠 **Microservices / Distributed Systems**                | Structured Logging ช่วยทำ Observability ได้ดี       |
| 🧠 **Enterprise Systems**                                 | รองรับ Logging to Seq, Kibana, Grafana              |
| 📦 **Background Services / Worker Services / Batch Jobs** | ส่ง log ไปยัง file หรือ dashboard เพื่อ monitor ได้ |
| 🧪 **ระบบที่ต้องการ Debug / Trace อย่างละเอียด**          | Logging มี context, method, และ arguments ได้       |

#

### 🔹 ตัวอย่าง Log แบบ Structured (Serilog)

```json
{
  "Timestamp": "2025-05-19T08:00:00Z",
  "Level": "Information",
  "MessageTemplate": "User {UserId} placed order {OrderId}",
  "Properties": {
    "UserId": 1234,
    "OrderId": 5678
  }
}
```

> 🧠 รูปแบบนี้ช่วยให้ระบบ Logging/Monitoring เช่น **Azure Log Analytics**, **Seq**, **Elasticsearch** หรือ **Grafana** วิเคราะห์ Log ได้ง่ายกว่าการ Log เป็นข้อความธรรมดา

#

### ✅ **ตารางเปรียบเทียบโดยรวม**

| Feature / Tool              | **NLog**                       | **Serilog**                                    | **System.Diagnostics** (`Trace`)        |
| --------------------------- | ------------------------------ | ---------------------------------------------- | --------------------------------------- |
| **ประเภท**                  | External Logging Framework     | External Logging Framework                     | Built-in Logging (BCL)                  |
| **Structured Logging**      | ❌ จำกัด (Log เป็น Text-Based)  | ✅ รองรับเต็มรูปแบบ (Structured, JSON ฯลฯ)      | ❌ ไม่มี                                 |
| **Configurable Targets**    | ✅ มากมาย (File, DB, Email ฯลฯ) | ✅ มากมายผ่าน Sink                              | ⚠️ จำกัด (TextWriter, EventLog เป็นต้น) |
| **Ease of Setup**           | ✅ ง่ายมาก                      | ✅ ง่าย พร้อม Extension มากมาย                  | ✅ ง่าย (เพราะเป็น built-in)             |
| **Performance**             | ✅ เร็วพอๆ กับ Serilog          | ✅ เร็วมาก                                      | ✅ เร็วพอสมควร                           |
| **Asynchronous Logging**    | ✅ รองรับ                       | ✅ รองรับ                                       | ❌ ไม่มีในตัว                            |
| **Structured Querying**     | ❌                              | ✅ Query ได้ เช่นใน Seq, Elasticsearch          | ❌ ไม่รองรับ                             |
| **Integration กับ ILogger** | ✅ ผ่าน Adapter                 | ✅ Native รองรับ `Microsoft.Extensions.Logging` | ✅ ผ่าน `TraceLoggerProvider` ได้        |
| **เหมาะกับ .NET Core / 8+** | ✅ แต่ต้องติดตั้งเพิ่ม          | ✅ ดีที่สุด                                     | ⚠️ Built-in แต่ไม่เหมาะงานใหญ่          |

#

### 🔧 **คำแนะนำการเลือกใช้งาน**

| Use Case                                                    | คำแนะนำ                                      |
| ----------------------------------------------------------- | -------------------------------------------- |
| ระบบ WebAPI, Microservice  | ✅ ใช้ **Serilog**                            |
| ระบบ WinForm, Windows Service หรือ legacy app               | ✅ ใช้ **NLog** หรือ **System.Diagnostics**   |
| ระบบเล็ก, ใช้งานภายใน, debug ขณะ dev                        | ✅ เริ่มจาก **System.Diagnostics**            | 

---

### 🔚 สรุป

> **Serilog คือ Logging Library สมัยใหม่** ที่เหมาะกับ .NET Core และ Microservice...
> รองรับ Structured Logging, ใช้งานง่าย, รองรับหลาย Output และมี Ecosystem ที่ดีมาก

| ต้องการอะไร?                      | ควรใช้                 |
| --------------------------------- | ---------------------- |
| Structured JSON Log               | **Serilog**            |
| Logging แบบธรรมดา, เขียนลงไฟล์    | **NLog**               |
| แค่ debug ตอน dev                 | **System.Diagnostics** |
| ใช้ร่วมกับ .NET ILogger ecosystem | **Serilog หรือ NLog**  |

หากคุณกำลังพัฒนา **Web API, Microservice, หรือ Distributed System** ด้วย .NET → Serilog คือ “ตัวเลือกที่ดีที่สุด” ณ ตอนนี้ 
 
#
### 1.4 เครื่องมือที่ต้องใช้:
1.  **Visual Studio 2022,VSCode** .NET 6.0 หรือใหม่กว่า
2.  **SQL Server Management Studio (SSMS)** หรือเครื่องมือจัดการฐานข้อมูล SQL Server อื่นๆ
3.  **SQL Server LocalDB** (มักจะติดตั้งมาพร้อมกับ Visual Studio)
4.  **Postman** หรือเครื่องมืออื่นสำหรับทดสอบ API

#

## 🛠️ คู่มือสร้างระบบ Product API ด้วย .NET 8 + Dapper + Clean Architecture

## 🔧 Stack ที่ใช้

* ASP.NET Core Web API
* Dapper (Micro ORM)
* MSSQL LocalDB
* Serilog (Logging)
* Clean Architecture
* SOLID Principles
* Middleware (Exception Handling, Logging, Response Wrapping)

---

## 1️⃣ โครงสร้างโฟลเดอร์ Project WebAPI ...

```
ProductAPI/
├── Program.cs
├── appsettings.json
├── Controllers/
│   └── ProductsController.cs
├── Domain/
│   ├── Entities/
│   │   └── Product.cs
│   └── Interfaces/
│       └── IProductRepository.cs
├── Application/
│   ├── DTOs/
│   │   ├── ProductCreateDTO.cs
│   │   ├── ProductResponseDTO.cs
│   │   └── ProductUpdateDTO.cs
│   ├── Interfaces/
│   │   └── IProductService.cs
│   └── Services/
│       └── ProductService.cs
├── Infrastructure/
│   └── Repositories/
│       └── ProductRepository.cs
├── Middleware/
│   ├── ExceptionMiddleware.cs
│   └── TODO...Middleware.cs
├── Common/
│   ├── ApiResponse.cs
└── Logs/
    └── log-*.txt
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

---

## 3️⃣ สร้าง Project แบบ Step-by-Step

### ✅ Step 1: สร้าง Solution และ Project

```bash
dotnet new webapi -n ProductAPI
cd ProductAPI 
```
### ✅ Step 2: สร้างโฟลเดอร์ตาม Clean Architecture

```bash
mkdir Domain Application Infrastructure Common
```
หรือสร้างโฟลเดอร์ใน VS2022/Explorer,VSCode:

```
ProductAPI/
├── Controllers/
├── Domain/              🧠 Entities, ValueObjects
├── Application/         🧠 DTOs, Interfaces, Services
├── Infrastructure/      🧠 Repositories, DB Access (Dapper)
├── Common/              🧠 Shared things (ApiResponse, Exceptions, Middlewares)
├── Program.cs, appsettings.json, etc.
```
 
#

## 4️⃣ Step-by-Step การเขียน Code (พร้อมอธิบายหลัก SOLID และ Clean Code)

![image](https://github.com/user-attachments/assets/9fb2839e-4609-469e-8cee-35d2698972eb)


### ✅ 4.1 ออกแบบ SQL Query (หมายเลข:1)
#### GetAll: 
```
SELECT * FROM Products
```
#### GetById:
```
SELECT * FROM Product2 WHERE Id = @Id
```
#### Create:
```
INSERT INTO Products (Name, Description, Price, Stock) 
VALUES (@Name, @Description, @Price, @Stock)
SELECT CAST(SCOPE_IDENTITY() as int)
```
#### Update:
```
UPDATE Products SET 
Name = @Name, 
Description = @Description, 
Price = @Price, 
Stock = @Stock 
WHERE Id = @Id
```

#### Delete:
```
DELETE FROM Products WHERE Id = @Id
```

### ✅ 4.2 สร้าง Repository (Infrastructure Layer) (หมายเลข:2)

เอา SQL Query ที่ได้ออกแบบไว้ ไปวางใน Code #
ใน Layer ดังนี้

📁 4.1.1 สร้าง interface... `Domain/Interfaces/IProductRepository.cs`

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

📁 4.1.2 สร้าง Class Repository... `Infrastructure/Repositories/ProductRepository.cs`
ทำหน้าที่ implement (นำไปใช้) ตาม interface IProductRepository ซึ่งเป็นแนวทางพื้นฐานของ Object-Oriented Programming (OOP) และใช้บ่อยในแนวคิด SOLID, Clean Architecture เพื่อแยกความรับผิดชอบและทำให้ระบบยืดหยุ่น
ดังนี้

```csharp
namespace ProductAPI.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString; 

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        { 
            //throw new SqlException("Simulated database error Repository Layer...");

            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }
         
        public async Task<Product> CreateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Products (Name, Description, Price, Stock) 
                    VALUES (@Name, @Description, @Price, @Stock);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
             
            var newId = await db.QuerySingleAsync<int>(sql, product);
            product.Id = newId; 
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"UPDATE Products SET 
                    Name = @Name, 
                    Description = @Description, 
                    Price = @Price, 
                    Stock = @Stock 
                    WHERE Id = @Id";
            var affected = await db.ExecuteAsync(sql, product);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var affected = await db.ExecuteAsync(
                "DELETE FROM Products WHERE Id = @Id", new { Id = id });
            return affected > 0;
        }
    }
}
```
**🧠 SOLID Principles:**
 
 * **Single Responsibility Principle (SRP):** `ProductRepository` รับผิดชอบเฉพาะการเข้าถึงข้อมูล Product ใน Database จัดการเฉพาะ Data Access.
 * **Dependency Inversion Principle (DIP):** Implement `IProductRepository` ที่กำหนดโดย Application Layer.
#




### ✅ 4.3 สร้าง Service Layer (Application Layer) (หมายเลข:3)

 

📁 4.3.1 สร้าง Entity (Domain Layer) `Domain/Entities/Product.cs`

```csharp
namespace ProductAPI.Domain.Entities
{
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
}


```
*หมายเหตุ:* การใช้ `string.Empty` และ `?` (nullable reference types) ช่วยในการจัดการ Nullability ใน C# 8.0+

**🧠 SOLID Principles:**

* **S (Single Responsibility)**: Entity นี้มีหน้าที่แทนข้อมูลเท่านั้น
* Clean Code: ชื่อ class/field ชัดเจน, ใช้ type ที่เหมาะสม

#

สร้าง Service Layer แล้ว Inject Repository Layer ผ่าน interface... IProductRepository
โดยมีการสร้าง Model DTOs (Data Transfer Objects) สำหรับ รับส่งข้อมูลระหว่าง Layer ด้วยดังนี้:
เราจะใช้ record แทน class

📁 4.3.2 สร้าง DTOs

📁 `Application/DTOs/ProductCreateDTO.cs`

```csharp
namespace ProductAPI.Application.DTOs
{
    public record ProductCreateDTO(
        [Required(ErrorMessage = "Product name is required."),StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
        string Name,

        string? Description,

        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]  
        decimal Price,

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        int Stock

   ); 

}

```
📁 `Application/DTOs/ProductResponseDTO.cs`

```csharp
namespace ProductAPI.Application.DTOs
{
    public record ProductResponseDTO( 
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        DateTime CreatedAt,
        DateTime? UpdatedAt
        );
}
```
📁 `Application/DTOs/ProductUpdateDTO.cs`

```csharp

namespace ProductAPI.Application.DTOs
{
    public record ProductUpdateDTO( 

      
        [Required(ErrorMessage = "Product ID is required for update.")]
        int Id,

        [Required(ErrorMessage = "Product name is required."),StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
        string? Name,

        string? Description,

        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]
        decimal? Price,

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        int? Stock

        );
}


```
* DTO = คลาสที่ใช้ส่ง/รับข้อมูล ไม่ใช่ตัวแทนของตารางในฐานข้อมูลโดยตรง

**🧠 SOLID Principles:**

* **I (Interface Segregation)**: แยก DTO ไม่ให้ใช้ Entity ตรง ๆ
* ใช้ `record` เพื่อความกระชับ, immutable

#

📁 4.3.4 สร้าง Interface + Service
แล้วเรียก ใช้งาน Repository Layer ผ่านการ Inject IProductRepository 

📁 `Application/Interfaces/IProductService.cs`

```csharp

namespace ProductAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto);
        Task<bool> UpdateAsync(ProductUpdateDTO productDto);
        Task<bool> DeleteAsync(int id);
    }
}


```

📁 `Application/Services/ProductService.cs`

```csharp

 // File: Application/Services/ProductService.cs


namespace ProductAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            //throw new InvalidOperationException("Simulated exception in Service Layer");
            return products.Select(p => MapToDTO(p));
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToDTO(product) : null;
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        { 
            var entity = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow
            };
             
            var newProduct = await _productRepository.CreateAsync(entity);
            return MapToDTO(newProduct);

        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO updateProductDto)
        { 
            var existingProduct = await _productRepository.GetByIdAsync(updateProductDto.Id);
            if (existingProduct is null)
                return false;

            existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
            existingProduct.Description = updateProductDto.Description ?? existingProduct.Description;
            existingProduct.Price = updateProductDto.Price ?? existingProduct.Price;
            existingProduct.Stock = updateProductDto.Stock ?? existingProduct.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            return await _productRepository.UpdateAsync(existingProduct);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private static ProductResponseDTO MapToDTO(Product product)
        {
            return new ProductResponseDTO(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.CreatedAt,
                product.UpdatedAt
              );
        }
    }
}

```
**🧠 SOLID Principles:**

* **S Single Responsibility Principle (SRP):** `ProductService` รับผิดชอบเฉพาะ Business Logic ที่เกี่ยวกับ Product และการประสานงานกับ Repository.
* **D (Dependency Inversion)**: ใช้ Interface แทนการผูกตรง
* **O (Open/Closed Principle (OCP))**: แก้ Service ได้โดยไม่กระทบ Layer อื่น
* Clean Code: Function name ชัดเจน, ใช้ LINQ แปลงข้อมูล
#



### ✅ 4.4 Controller (API Layer) (หมายเลข:4)

ใน Layer นี้ เรียกใช้งาน ProductService Layer ผ่านการ Inject IProductService 

📁 `Controllers/ProductsController.cs`

```csharp


namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger ,IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
        { 
            try
            {
                var products = await _productService.GetAllAsync();

                var response = ApiResponse<IEnumerable<ProductResponseDTO>>.SuccessResponse(
                    data: products,
                    message: "Products retrieved successfully",
                    statusCode: StatusCodes.Status200OK
                );

                //throw new Exception(" Simulated Exception in Controller *********** ");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllProducts");
                var errorResponse = ApiResponse<object>.FailResponse(
                    message: "An unexpected error occurred while fetching products.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    error: ex.Message // หรือจะรวมกับ StackTrace ก็ได้
                );

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }

        }

        [HttpGet("api/Products2")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDTO>>>> Get()
        {
            var products = await _productService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ProductResponseDTO>>.SuccessResponse(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product != null
                ? Ok(ApiResponse<ProductResponseDTO>.SuccessResponse(product))
                : NotFound(ApiResponse<ProductResponseDTO>.FailResponse("Product not found", 404));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Post([FromBody] ProductCreateDTO dto)
        { 

            var product = await _productService.CreateAsync(dto);

            //throw new Exception("Test Exception CreatedAtAction -----------------------");
            return CreatedAtAction(nameof(Get), new { id = product.Id },  ApiResponse<ProductResponseDTO>.SuccessResponse(product, "Product created", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Put(int id, [FromBody] ProductUpdateDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<object>.FailResponse("Invalid product ID", 400));

            var result = await _productService.UpdateAsync(dto);
            return result
                ? Ok(ApiResponse<object>.SuccessResponse(null, "Product updated"))
                : NotFound(ApiResponse<object>.FailResponse("Product not found", 404));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return result
                ? Ok(ApiResponse<object>.SuccessResponse(null, "Product deleted"))
                : NotFound(ApiResponse<object>.FailResponse("Product not found", 404));
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
    
**🧠 SOLID Principles:**

* **D (Dependency Inversion)**: Controller ไม่ขึ้นกับ Implementation
* Clean Code: Response เป็น `ApiResponse`, แยก logic ไป Service

#

### ✅ 4.5 Register Service และรวมถึง การเพิ่ม Class อื่น เช่น Middleware + Api Response Wrapper (Common) (หมายเลข:5)

📁 4.5.1 Response Wrapper ... `Common/ApiResponse.cs`

```csharp
namespace ProductAPI.Common
{
    public record ApiResponse<T>(int StatusCode, bool Success, string Message, T? Data, string? Error = null)
    {
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200) => new(statusCode, true, message, data);

        public static ApiResponse<T> FailResponse(string message = "Failed", int statusCode = 400, string? error = null) => new(statusCode, false, message, default, error);
    }

}
```

## ✨ ResponseBody (Wrapped)

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

📁 4.5.2 `Middleware/ExceptionMiddleware.cs`

```csharp

namespace ProductAPI.Middleware
{
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
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception ex)
            {
                // Get Action Name from endpoint
                var endpoint = context.GetEndpoint();
                var actionName = endpoint?.DisplayName ?? "UnknownAction";

                // Read Request Body
                var requestBody = await ReadRequestBodyAsync(context.Request);

                // StackTrace
                var st = new StackTrace(ex, true);
                var frame = st.GetFrames()?.FirstOrDefault(f => f.GetFileLineNumber() > 0);
                var methodInfo = frame?.GetMethod();
                var declaringType = methodInfo?.DeclaringType;

                // Extract original method name (handle async state machine)
                var originalMethodName = declaringType?.Name;
                string methodName;
                if (!string.IsNullOrWhiteSpace(originalMethodName) && originalMethodName.Contains("<") && originalMethodName.Contains(">"))
                {
                    var start = originalMethodName.IndexOf("<") + 1;
                    var end = originalMethodName.IndexOf(">");
                    methodName = originalMethodName.Substring(start, end - start);
                }
                else
                {
                    methodName = methodInfo?.Name ?? "UnknownMethod";
                }

                // Get class name (real class, not compiler generated)
                var className = declaringType?.DeclaringType?.Name ?? declaringType?.Name ?? "UnknownClass";
                var lineNumber = frame?.GetFileLineNumber();

                // Compose Error Detail
                var errorDetail = $"Class: {className}, Method: {methodName}, Line: {lineNumber}, Action: {actionName}, Request: {requestBody}";

                // Log
                _logger.LogError(ex, "Exception occurred: {ErrorDetail}", errorDetail);

                // Return API JSON Response

                context.Items["ExceptionHandled"] = true;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ApiResponse<object>.FailResponse("Internal Server Error", 500, errorDetail);               

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            try
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
            catch
            {
                return "Unable to read request body.";
            }
        }

    }
} 

```

📁 4.5.3 Register Service ใน `Program.cs`

```csharp
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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

```

📁 4.5.4 `appsettings.json`

```csharp
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
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

#
## ✅ สรุป Feature ที่ได้ใน Starter Template นี้:

| Feature                        | ✅ ครบ |
| ------------------------------ | ----- |
| Clean Architecture             | ✅     |
| Dapper ORM                     | ✅     |
| Repository Pattern             | ✅     |
| DTO Mapping                    | ✅     |
| Middleware: Exception Handling | ✅     |
| Response Wrapping  | ✅     |
| Serilog Logging                | ✅     |
| Dependency Injection           | ✅     |
| Configurable Connection String | ✅     |
| Swagger UI (Dev Mode)          | ✅     |

#
# ด้วยความปรารถนาดีจากทีม Core System

