# 📚 ขั้นตอน การ Implement

### ✅ 1. Download หรือ Clone Project https://github.com/nuchit2019/Product_API_Starter_Kit.git
### ✅ 2. แตก Zip ... เปิด Project ... แล้ว Connect ฐานข้อมูล, สร้างฐานข้อมูล, สร้าง Table ตามรูป
#### ✅ 2.1. Connect ฐานข้อมูล, สร้างฐานข้อมูล ...
#### ![image](https://github.com/user-attachments/assets/690323b9-2da4-4358-ba68-ceb770a1a20c)
#### ![image](https://github.com/user-attachments/assets/d3157aa8-dc40-4ff4-98f4-7d125a6a9890)


#### ✅ 2.2. สร้างตาราง...
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
 ![image](https://github.com/user-attachments/assets/14b083e7-9d90-4643-a95c-d2326e3baf31)


### ✅ 3. เปิด Task List ... แล้ว Implement ตาม Layer 
![image](https://github.com/user-attachments/assets/946fa321-dbf4-4b3b-af4e-41e384fa4d2a)

### ✅ 4. ดับเบิ้ลคลิก บน Task ที่ต้องการ Implement ... จะเปิด หน้าต่าง Code ... 
![image](https://github.com/user-attachments/assets/fe175e58-704f-4e93-aa98-ef2f61ced9bf)

### ✅ 5. ลงมือเขียน Code ... ตามรูป Step-by-Step การเขียน Code...
#### ✅ Step-by-Step การเขียน Code...

![image](https://github.com/user-attachments/assets/9fb2839e-4609-469e-8cee-35d2698972eb)
### ✅ 6. จบขั้นตอน
#

## 👨‍💻 Author

> Starter Template โดยทีมพัฒนา .NET (Core System) 
 
#
