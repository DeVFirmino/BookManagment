/*
------------------------------------------------------------
 BORROW MODEL
------------------------------------------------------------
Purpose
• Represents the relationship between a user and a book when a borrowing occurs.
• Tracks which user borrowed which book, and when it was borrowed/returned.

Database Mapping
• Primary Key → Id
• Foreign Keys → UserId (UserModel), BookId (BooksModel)
• Navigation Properties:
    - User → Links to the user who borrowed the book.
    - Book → Links to the borrowed book.

Main Properties
• BorrowDate → Date when the book was borrowed (default = current date).
• ReturnDate → Date when the book was returned (nullable until returned).

Usage
• Used by BorrowController and BorrowService to manage borrowing operations.
• Enables reporting of active (pending) and completed (returned) borrows.
------------------------------------------------------------
*/

using System.Text.Json.Serialization;

namespace BooksApi.Models;

public class BorrowModel
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    [JsonIgnore] 
    public UserModel User { get; set; }

    public int BookId { get; set; }
    [JsonIgnore]

    public BooksModel Book { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; }
}