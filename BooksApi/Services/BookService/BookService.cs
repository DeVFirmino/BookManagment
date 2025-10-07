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
    
    public BookService(AppDbContext context)
    {
        _context = context;
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


            // var book = new BooksModel
            // {
            //     Title = bookCreationDto.Title,
            //     Cover = fileName,
            //     Author = bookCreationDto.Author,
            //     Description = bookCreationDto.Description,
            //     Stock = bookCreationDto.Stock,
            //     DatePublished = bookCreationDto.DatePublished,
            //     Genre = bookCreationDto.Genre,
            // }; 

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
}