# 🛒 OnlineStore API

This is the **backend API** for the OnlineStore project.  
It provides authentication, authorization, and business logic for managing the online store system.  
The API is built with **.NET Core**, uses **SQL Server** as the database, and follows **Clean Architecture** principles.

---

## 🚀 Tech Stack

- **.NET Core Web API**
- **SQL Server** (Stored Procedures)
- **Dapper** (Micro ORM)
- **JWT Authentication & Authorization**
- **Mapster** (Object Mapping)
- **Clean Architecture** (Domain, Application, Infrastructure, API layers)

---

## 📂 Project Structure

- **OnlineStore.Api** → Presentation layer (Controllers, Middlewares, Utils, etc.)
- **OnlineStore.Application** → Application logic (DTOs, Services, Requests, Responses, etc.)
- **OnlineStore.Domain** → Core domain (Entities, Interfaces, Enums, etc.)
- **OnlineStore.Infrastructure** → Data access (Repositories, Database connection, etc.)

---

## 🔑 Features (In Progress)

- [x] JWT Authentication
- [x] Dapper integration with Stored Procedures
- [x] User & Role Management
- [x] Category Managment
- [ ] Product Management (CRUD)
- [ ] Order Management
- [ ] File Uploads (Images, etc.)

---

## 📸 Screenshot
![App Screenshot](/screenshots/1.png)
