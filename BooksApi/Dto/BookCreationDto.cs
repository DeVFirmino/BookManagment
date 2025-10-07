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