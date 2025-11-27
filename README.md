# Book Management

An ASP.NET Core 9.0 MVC application for managing a small library. Users can browse the catalog, borrow and return books, manage their profiles and addresses, and administrators can monitor inventory and generate Excel-style reports.

## Features
- **Public catalog** with search and detailed book pages that include cover images and publication data.
- **User authentication** with session-based login/logout and active/inactive user handling.
- **Borrowing workflow** to check out and return books, with stock tracking and per-user status visibility.
- **Address and profile management** for storing user details alongside library activity.
- **Reports** that export borrowing and inventory data via ClosedXML.
- **Random motivational quotes** on the home page using Quotable/DummyJSON as fallbacks.

## Tech stack
- **Framework:** ASP.NET Core 9.0 MVC
- **Data access:** Entity Framework Core 9 with SQL Server
- **Object mapping:** AutoMapper
- **Exports:** ClosedXML
- **Session management:** Built-in ASP.NET Core session middleware

## Getting started
### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server instance reachable from the app

### Configuration
1. Copy `BooksApi/appsettings.json` to `BooksApi/appsettings.Development.json` (already present) if you want environment-specific settings.
2. Update `ConnectionStrings:DefaultConnection` to point to your SQL Server.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BookDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
}
```

### Restore & database setup
```bash
cd BooksApi
dotnet restore
dotnet ef database update
```
The `dotnet ef` command applies migrations from the `Migrations/` folder to create the required tables (`Books`, `Users`, `Addresses`, `Borrow`).

### Run the app
```bash
cd BooksApi
dotnet run
```
The site defaults to `https://localhost:5001` (or the console output port). Static assets (covers, CSS, JS) are served from `wwwroot`.

## Project structure
- `BooksApi/Program.cs` – dependency injection setup, session middleware, routing, static assets.
- `BooksApi/Data/AppDbContext.cs` – EF Core context for books, users, addresses, and borrow records.
- `BooksApi/Models/` – entity models such as `BooksModel`, `UserModel`, `AddressModel`, `BorrowModel`.
- `BooksApi/Controllers/` – MVC controllers for home, books, borrowing, users, employers, reports, and clients.
- `BooksApi/Services/` – domain services for authentication, session handling, borrowing logic, reporting, and CRUD operations.
- `BooksApi/Views/` – Razor views for pages and components; `wwwroot/` contains static assets.

## Useful commands
- `dotnet test` – run any available tests in the solution.
- `dotnet ef migrations add <Name>` – add a new database migration.
- `dotnet ef database update` – apply migrations to the configured database.

## Licensing
Provide licensing terms here if applicable.
