/*
------------------------------------------------------------
 BOOK MODEL
------------------------------------------------------------
Purpose
• Represents the book entity stored in the system.
• Contains all essential information about a book available for borrowing.

Database Mapping
• Primary Key → Id
• Related Table → Borrow (one-to-many relationship with BorrowModel)

Main Properties
• Title, Description, Author, Genre → Required descriptive fields.
• Cover → Stores the book cover image filename.
• DatePublished → Year of publication.
• Stock → Number of copies available.
• RegisterDate / RegisterChange → Track creation and last modification timestamps.

Usage
• Used by BookController and BookService for book management.
• Connected to BorrowModel to track which user borrowed the book.
------------------------------------------------------------
*/
using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models;

public class BooksModel
{

    [Key] //DataAnottation[[pk]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Cover { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public string Genre { get; set; } = string.Empty;
    [Required]
    public int DatePublished { get; set; }
    
    public List<BorrowModel> Borrow { get; set; }

    
    [Required]
    public int Stock { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    
    public DateTime RegisterChange { get; set; } = DateTime.Now;
    
     
}
