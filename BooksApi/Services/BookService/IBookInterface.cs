// IBookInterface.cs
// ---------------------------------------------------------
// INTERFACE CONCEPT:
// An interface is like a contract/blueprint. 
// It defines *what methods must exist* but not *how they work*. 
// Any class that implements this interface (e.g., BookService)
// must provide the actual logic for these methods.
//
// WHY USE IT?
// - Abstraction: hides implementation details.
// - Flexibility: allows swapping implementations easily.
// - Testability: great for dependency injection and unit testing.
//
// THIS INTERFACE (IBookInterface):
// - SearchBooks()     -> Returns all books
// - VerifyIfExists()  -> Checks if a book already exists (Title, Author, Date)
// - Register()        -> Adds a new book with a cover image
// - SearchBookById()  -> Finds a book by ID
// - Edit()            -> Updates book info and optionally its cover
// ---------------------------------------------------------
using BooksApi.Dto;
using BooksApi.Models;

namespace BooksApi.Services.BookService;

public interface IBookInterface
{
    Task<List<BooksModel>> SearchBooks();
    bool VerifyIfExists(BookCreationDto bookCreationDto);
    Task<BooksModel> Register(BookCreationDto bookCreationDto, IFormFile photo);
    Task<BooksModel> SearchBookById(int? id);
    Task<BorrowModel> SearchBookById(int? id, UserModel userSession);
    Task<BooksModel> Edit(BookEditDto bookEditDto, IFormFile? photo);

    Task<List<BooksModel>> FindBooksByFilter(string search);
    
    
    
    
}