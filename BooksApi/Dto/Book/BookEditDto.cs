/*
------------------------------------------------------------
 BOOK EDIT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents the data structure used to edit or update an existing book record.
• Transfers validated input data between the view (form) and the service layer.
• Keeps the editing process secure and separated from the database entity.

Validation
• Uses [Required] attributes to ensure that all key fields are provided.
• Provides user-friendly error messages for form validation.

Usage
• Used in BookController (Edit actions) to handle update requests.
• Supports editing forms in the view (Book/Edit.cshtml).
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;

namespace BooksApi.Dto;

public class BookEditDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Insert the title!")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Insert the description!")]
    public string Description { get; set; } = string.Empty;
     
    [Required(ErrorMessage = "Insert the author!")]
    public string Author { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Insert the genre!")]
    public string Genre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Insert the date published!")]
    public int DatePublished { get; set; }
    
    [Required(ErrorMessage = "Insert the quantity in stock!")]
    public int Stock { get; set; }
    
    public string Cover { get; set; } = string.Empty;
}