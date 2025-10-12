/*
------------------------------------------------------------
 PROFILE ENUM
------------------------------------------------------------
Purpose
• Defines the possible user roles (profiles) within the system.
• Provides strong typing instead of using plain strings or numbers.

Values
• Administrator = 1  → Has full access to manage users, books, and reports.
• Client        = 0  → Regular user who can borrow and view books.

Usage
• Used throughout the system (e.g., UserController, Filters, DTOs)
  to control access levels and display user roles consistently.
------------------------------------------------------------
*/
namespace BooksApi.Enums
{
    public enum ProfileEnum
    {
        Administrator = 1,
        Client = 0
    }
}