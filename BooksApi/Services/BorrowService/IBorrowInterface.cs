/*
------------------------------------------------------------
 BORROW SERVICE INTERFACE (IBorrowInterface)
------------------------------------------------------------
Purpose  
• Defines the contract for all borrow-related operations  
  — ensuring consistent method signatures across implementations.

Main Methods  
• Borrow(bookId) → Creates a new borrow record for a given book.  
• SearchBorrowsFilter(user, search) → Filters user borrows by title or author.  
• SearchBorrows(user) → Retrieves all borrows linked to a specific user.  
• Return(id) → Marks a borrowed book as returned and updates its stock.  
• SearchAllBorrows(type) → Fetches all returned or pending borrows depending on the type parameter.

Usage  
• Implemented by `BorrowService`, which provides the actual logic.  
• Used by controllers (e.g., BorrowController, ReportController)  
  to manage borrowing workflows.
------------------------------------------------------------
*/

using BooksApi.Models;

namespace BooksApi.Services.BorrowService
{
    public interface IBorrowInterface
    {
        Task<ResponseModel<BorrowModel>> Borrow(int bookId);
        Task<List<BorrowModel>> SearchBorrowsFilter(UserModel sessionUser, string search);
        Task<List<BorrowModel>> SearchBorrows (UserModel sessionUser);
        
        Task<BorrowModel> Return(int id);
        Task<List<BorrowModel>> SearchAllBorrows(string type = null);
    }
}