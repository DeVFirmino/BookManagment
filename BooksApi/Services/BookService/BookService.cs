// BookService.cs
// ---------------------------------------------------------
// This service handles all operations related to books:
// - SearchBooks() -> Returns all books from database
// - SearchBookById() -> Returns one book by its ID
// - VerifyIfExists() -> Checks if a book already exists (by Title, Author, DatePublished)
// - Register() -> Creates a new book, saves its image in /wwwroot/image, and stores file name in DB
// - Edit() -> Updates book info; if new image is uploaded, saves it and deletes the old one
// - PathWay() / SaveImageAsync() -> Helper methods to generate unique file name and store image on disk
// - DeleteImage() -> Removes old cover image file
//
// Dependencies: AppDbContext (EF Core), AutoMapper (DTO → Model mapping),
// IWebHostEnvironment (for WebRootPath).
// ---------------------------------------------------------

using AutoMapper;
using BooksApi.Data;
using BooksApi.Dto;
using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Services.BookService;

public class BookService : IBookInterface
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private string _serverWay;

    public BookService(AppDbContext context, IWebHostEnvironment system, IMapper mapper)
    {
        _context = context;
        _serverWay = system.WebRootPath;
        _mapper = mapper;

    }

    public async Task<List<BooksModel>> SearchBooks()
    {
        try
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool VerifyIfExists(BookCreationDto bookCreationDto)
    {
        return _context.Books.Any(book =>
            book.Title == bookCreationDto.Title &&
            book.Author == bookCreationDto.Author &&
            book.DatePublished == bookCreationDto.DatePublished
        );
    }

    public async Task<BooksModel> Register(BookCreationDto bookCreationDto, IFormFile photo)
    {
        try
        {
            // Usar a função reutilizável
            var fileName = await SaveImageAsync(photo);

            var book = _mapper.Map<BooksModel>(bookCreationDto);
            book.Cover = fileName;

            _context.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }


    public async Task<BooksModel> SearchBookById(int? id)
    {
        try
        {
            var book = await _context.Books.FirstOrDefaultAsync(l => l.Id == id);

            return book;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }



    public async Task<BooksModel> Edit(BookEditDto bookEditDto, IFormFile? photo)
    {
        try
        {
            var existingBook = await _context.Books.FindAsync(bookEditDto.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            string oldCover = existingBook.Cover;

            // Atualizar dados básicos
            existingBook.Title = bookEditDto.Title;
            existingBook.Author = bookEditDto.Author;
            existingBook.Description = bookEditDto.Description;
            existingBook.Genre = bookEditDto.Genre;
            existingBook.DatePublished = bookEditDto.DatePublished;
            existingBook.Stock = bookEditDto.Stock;
            existingBook.RegisterChange = DateTime.Now;

            // Se nova imagem foi enviada
            if (photo != null && photo.Length > 0)
            {
                // Salvar nova imagem
                var newFileName = await SaveImageAsync(photo);
                existingBook.Cover = newFileName;

                // Deletar imagem antiga
                DeleteImage(oldCover);
            }

            _context.Update(existingBook);
            await _context.SaveChangesAsync();

            return existingBook;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<BooksModel>> FindBooksByFilter(string search)
    {
        try
        {
            var books = await _context.Books
                .Where(book => book.Title.Contains(search) || book.Author.Contains(search))
                .ToListAsync();

            return books;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<BooksModel>> FindBooks()
    {
        try
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public string PathWay(IFormFile photo)
    {

        var uniqueCode = Guid.NewGuid().ToString();
        var fileName = photo.FileName.Replace(" ", "").ToLower() + uniqueCode + ".png";

        string imageDirectory = Path.Combine(_serverWay, "image");
        string fullPath = Path.Combine(imageDirectory, fileName);


        // Criar diretório se não existir
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        // Salvar arquivo
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            photo.CopyToAsync(stream);
        }

        return imageDirectory;

    }

    private async Task<string> SaveImageAsync(IFormFile photo)
    {
        var uniqueCode = Guid.NewGuid().ToString();
        var fileName = photo.FileName.Replace(" ", "").ToLower() + uniqueCode + ".png";

        string imageDirectory = Path.Combine(_serverWay, "image");
        string fullPath = Path.Combine(imageDirectory, fileName);

        // Criar diretório se não existir
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        // Salvar arquivo
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await photo.CopyToAsync(stream);
        }

        return fileName;
    }

    private void DeleteImage(string fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            string imagePath = Path.Combine(_serverWay, "image", fileName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }

    public async Task<BorrowModel> SearchBookById(int? id, UserModel userSession)
    {
        try
        {
            // User not logged in
            if (userSession == null)
            {
                var borrowNoUser = await _context.Borrow
                    .Include(b => b.Book)
                    .FirstOrDefaultAsync(b => b.BookId == id);

                if (borrowNoUser == null)
                {
                    var book = await SearchBookById(id); // <- your other overload that returns BooksModel

                    var borrowFromDb = new BorrowModel
                    {
                        Book = book,
                        User = null
                    };

                    return borrowFromDb;
                }

                return borrowNoUser;
            }

            // User logged in
            var borrow = await _context.Borrow
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookId == id 
                                          && b.ReturnDate == null 
                                          && b.User.Id == userSession.Id);

            if (borrow == null)
            {
                var book = await SearchBookById(id); // 

                var borrowFromDb = new BorrowModel
                {
                    Book = book,
                    User = userSession
                };

                return borrowFromDb;
            }

            return borrow;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

} 
