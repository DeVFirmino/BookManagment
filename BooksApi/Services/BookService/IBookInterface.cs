using BooksApi.Dto;
using BooksApi.Models;

namespace BooksApi.Services.BookService;

public interface IBookInterface
{
     
          
     Task<List<BooksModel>> SearchBooks();
     bool VerifyIfExists(BookCreationDto bookCreationDto);
     Task<BooksModel> Register(BookCreationDto bookCreationDto, IFormFile photo);
}    