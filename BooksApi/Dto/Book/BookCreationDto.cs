// BookCreationDto.cs
// ---------------------------------------------------------
// DTO CONCEPT:
// A DTO (Data Transfer Object) is a simple object used to 
// transfer data between layers (e.g., Controller -> Service).
// It does not contain business logic, only properties.
//
// WHY USE IT?
// - Keeps models clean and avoids exposing internal entities
// - Validates input data before reaching the database
// - Makes the application more secure and structured
//
// THIS DTO (BookCreationDto):
// - Title          -> Book title
// - Description    -> Book description
// - Author         -> Book author
// - Genre          -> Book genre
// - DatePublished  -> Year of publication
// - Stock          -> Quantity available
// ---------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace BooksApi.Dto;

public class BookCreationDto
{
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
 
}