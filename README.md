# 🛒 Talabat — E-Commerce Backend API

> A production-ready RESTful API for an e-commerce platform, built with **ASP.NET Core** following **Clean Architecture** principles.

---

## 🚀 Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# / .NET |
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Auth | JWT Bearer Tokens |
| Architecture | Clean Architecture (Onion) |
| API Docs | Swagger / OpenAPI |

---

## 🏗️ Architecture

The project follows **Clean Architecture** with a clear separation of concerns across 4 layers:

```
Talabat-Project/
├── Talabat.Api          # Presentation layer — Controllers, Middlewares
├── Talabat.Core         # Domain layer — Entities, Interfaces, DTOs
├── Talabat.Service      # Business logic layer — Services, Use Cases
└── Talabat.Repository   # Data layer — EF Core, Repos, DbContext
```

Each layer depends only on the layer beneath it — **no circular dependencies**.

---

## ✨ Features

- 🔐 **Authentication & Authorization** — Register / Login with JWT
- 📦 **Product Management** — CRUD with filtering, sorting & pagination
- 🗂️ **Category Management** — Nested categories support
- 🛒 **Shopping Cart** — Add, update, remove items
- 📋 **Order Management** — Place orders, track status
- 💳 **Payment Integration** — Stripe payment gateway
- 🖼️ **Image Upload** — Product images support
- 📄 **Swagger UI** — Full API documentation

---

## 📡 API Endpoints (Sample)

### Auth
```
POST   /api/account/register
POST   /api/account/login
GET    /api/account/currentuser
```

### Products
```
GET    /api/products               # Get all products (filter/sort/paginate)
GET    /api/products/{id}          # Get single product
GET    /api/products/brands        # Get all brands
GET    /api/products/types         # Get all types
```

### Basket
```
GET    /api/basket?id={id}
POST   /api/basket
DELETE /api/basket?id={id}
```

### Orders
```
POST   /api/orders
GET    /api/orders                 # Get user orders
GET    /api/orders/{id}
GET    /api/orders/deliverymethods
```

---

## ⚙️ Getting Started

### Prerequisites
- .NET 7 SDK
- SQL Server
- Redis (for basket)

### Run Locally

```bash
# Clone the repo
git clone https://github.com/ShrifGamal/Talabat-Project.git
cd Talabat-Project

# Update connection string in appsettings.json
# Then run migrations
dotnet ef database update

# Run the API
dotnet run --project Talabat.Api
```

Visit `https://localhost:5001/swagger` to explore the API.

---

## 🧱 Design Patterns Used

- **Repository Pattern** — abstracted data access
- **Unit of Work** — consistent transactions across repos
- **Generic Repository** — reusable CRUD operations
- **Specification Pattern** — dynamic query building
- **Dependency Injection** — throughout all layers

---

## 👤 Author

**Shrif Gamal**
[GitHub](https://github.com/ShrifGamal)
