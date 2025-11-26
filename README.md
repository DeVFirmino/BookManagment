# BooksApi (Book Management)

An ASP.NET Core MVC application for managing a library of books with borrowing, reporting, and user account workflows. It uses Entity Framework Core with SQL Server, AutoMapper, and server-side sessions for authentication-aware pages.

## Features
- Book catalog with cover uploads, filtering, and detailed pages
- Borrowing workflow that tracks book/user relationships and borrowing status
- User registration and management, including login/logout and session-backed access control
- Reporting pages for library activity and borrowing metrics
- Razor views with dynamic layouts for guests versus authenticated users

## Project structure
- `Program.cs` – Dependency injection setup, EF Core configuration, AutoMapper, session support, and MVC pipeline.
- `Controllers/` – MVC controllers for books, users, borrowing, reports, and home/login flows.
- `Services/` – Business logic for books, users, authentication, borrowing, reporting, and session handling.
- `Models/` and `Dto/` – Entity models, DTOs, and view models used across the app.
- `Data/` – `AppDbContext` and EF Core migrations for the SQL Server schema.
- `Views/` and `wwwroot/` – Razor pages plus static assets (images, CSS, JS).

## Getting started
1. **Prerequisites**: .NET 8 SDK, SQL Server instance reachable from the app, and the [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) tool for migrations.
2. **Configure the database**: Update `ConnectionStrings:DefaultConnection` in `BooksApi/appsettings.json` to point to your SQL Server.
3. **Restore and build**:
   ```bash
   dotnet restore
   dotnet build
   ```
4. **Apply migrations** (creates the schema defined in `Migrations/`):
   ```bash
   dotnet ef database update --project BooksApi
   ```
5. **Run the app**:
   ```bash
   dotnet run --project BooksApi
   ```
   The site defaults to the `/Home/Index` page with optional search; authenticated users get the main layout and can manage books and borrowing.

## Helpful next steps
- Review `Controllers/HomeController.cs` for the login flow, session usage, and quote-loading logic for the home page.
- Explore `Services/BookService` and `Services/BorrowService` to see how book management and borrowing rules are enforced.
- Inspect the Razor views under `Views/` to understand how TempData messages and layout switching appear in the UI.

