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
    
    [Required]
    public int Stock { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    
    public DateTime RegisterChange { get; set; } = DateTime.Now;
    
     
}
