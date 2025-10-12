/*
------------------------------------------------------------
 APPLICATION DATABASE CONTEXT (AppDbContext)
------------------------------------------------------------
Purpose
• Serves as the Entity Framework Core database context for the application.
• Maps domain models (entities) to database tables via DbSet<T>.

Configuration
• Inherits from DbContext.
• Configured through dependency injection with DbContextOptions<AppDbContext>.

Entities
• Books     → Table for book records (BooksModel)
• Users     → Table for application users (UserModel)
• Addresses → Table for user addresses (AddressModel)
• Borrow    → Table for book borrow/return records (BorrowModel)

Usage
• Central point for database CRUD operations across all services.
------------------------------------------------------------
*/

using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data
{
    public class AppDbContext : DbContext
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        } 
        public DbSet<BooksModel> Books {get; set;}
        public DbSet<UserModel> Users { get; set; }
        
        public DbSet<AddressModel> Addresses { get; set; }
        
        public DbSet<BorrowModel> Borrow { get; set; }
         
        
    }
}