/*
------------------------------------------------------------
 BORROW SERVICE (BorrowService)
------------------------------------------------------------
Purpose
• Handles all borrow/return operations and stock adjustments.
• Orchestrates DB access (via AppDbContext) and book lookups
  (via IBookInterface) using the current logged-in session.

Dependencies
• IBookInterface       → fetches books for borrow/return logic
• AppDbContext         → persistence for Borrow and Book entities
• ISessionInterface    → identifies the logged-in user

Key Methods
• Borrow(bookId)                 → Creates a borrow record for the logged user and decrements stock.
• Return(id)                     → Sets ReturnDate, saves, and increments stock.
• SearchBorrowsFilter(user, s)   → User’s borrows filtered by book title/author.
• SearchBorrows(user)            → All borrows for a specific user.
• SearchAllBorrows(type)         → Returned (type == null) or pending (type != null) borrows.
• DecreaseStock(book) / IncreaseStock(book)
                                 → Updates book inventory on borrow/return.

Notes
• Validates session before borrowing/returning.
• Throws with clear messages when records aren’t found.
• Includes related entities (Book/User) to support views and reports.
------------------------------------------------------------
*/

using BooksApi.Data;
using BooksApi.Models;
using BooksApi.Services.BookService;
using BooksApi.Services.SessionService;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Services.BorrowService;

public class BorrowService : IBorrowInterface
{
    private readonly IBookInterface _bookInterface;
    private readonly AppDbContext _context;
    private readonly ISessionInterface _sessionInterface;

    public BorrowService(IBookInterface bookInterface, AppDbContext context, ISessionInterface sessionInterface)
    {
        _bookInterface = bookInterface;
        _context = context;
        _sessionInterface = sessionInterface;
    }

    public async Task<ResponseModel<BorrowModel>> Borrow(int bookId)
    { 
        ResponseModel<BorrowModel> response = new ResponseModel<BorrowModel>();
        try
        {
            var sessionUser = _sessionInterface.FindSession();

            if (sessionUser == null)
            {
                response.Status = false;
                response.Message = "You must be logged in to borrow a book!";
                return response;
            }

            var book = await _bookInterface.SearchBookById(bookId);

            if (book == null)
            {
                response.Status = false;
                response.Message = "Book not found!";
                return response;
            }

            if (book.Stock <= 0)
            {
                response.Status = false;
                response.Message = "Book unavailable for borrowing!";
                return response;
            }

            var borrow = new BorrowModel
            {
                UserId = sessionUser.Id,
                BookId = book.Id,
                Book = book
            };

            _context.Add(borrow);
            await _context.SaveChangesAsync();

            var updatedBook = await DecreaseStock(book);

            response.Data = borrow;

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<BorrowModel>> SearchBorrowsFilter(UserModel sessionUser, string search)
    {
        try
        {
            var filteredBorrows = await _context.Borrow
                .Include(u => u.User)
                .Include(b => b.Book)
                .Where(b => b.UserId == sessionUser.Id &&
                            (b.Book.Title.Contains(search) || b.Book.Author.Contains(search)))
                .ToListAsync();

            return filteredBorrows;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<BorrowModel>> SearchBorrows(UserModel sessionUser)
    {
        try
        {
            var userBorrows = await _context.Borrow
                .Where(b => b.UserId == sessionUser.Id)
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();

            return userBorrows;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BooksModel> DecreaseStock(BooksModel book)
    {
        book.Stock--;
        _context.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
    
    public async Task<BooksModel> IncreaseStock(BooksModel book)
    {
        book.Stock++;
        _context.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task<BorrowModel> Return(int id)
    {
        try
        {
            var borrow = await _context.Borrow
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrow == null)
            {
                throw new Exception("Borrow record not found!");
            }

            borrow.ReturnDate = DateTime.Now;

            _context.Update(borrow);
            await _context.SaveChangesAsync();

            var updatedBook = await IncreaseStock(borrow.Book);

            return borrow;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<BorrowModel>> SearchAllBorrows(string type = null)
    {
        try
        {
            if (type == null)
            {
                var returnedBorrows = await _context.Borrow
                    .Include(book => book.Book)
                    .Include(user => user.User)
                    .Where(borrow => borrow.ReturnDate != null)
                    .ToListAsync();

                return returnedBorrows;
            }
            else
            {
                var pendingBorrows = await _context.Borrow
                    .Include(book => book.Book)
                    .Include(user => user.User)
                    .Where(borrow => borrow.ReturnDate == null)
                    .ToListAsync();

                return pendingBorrows;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

   

