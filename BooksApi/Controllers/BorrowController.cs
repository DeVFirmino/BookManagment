/*
------------------------------------------------------------
 BORROW CONTROLLER
------------------------------------------------------------
• Responsible for managing the book borrowing and returning 
  operations within the system.

• Main functionalities:
  - Allow logged users to borrow books available in the library.
  - Handle the process of returning borrowed books.
  - Display basic information related to current borrow actions.

• Access control:
  - Requires user authentication via session validation.
  - Redirects unauthenticated users to the Login page.
------------------------------------------------------------
*/

using Microsoft.AspNetCore.Mvc;
using BooksApi.Services.BorrowService;
using BooksApi.Services.BookService;
using BooksApi.Services.SessionService;

namespace BooksApi.Controllers
{
    public class BorrowController : Controller
    {
        private readonly ISessionInterface _sessionInterface;
        private readonly IBookInterface _bookInterface;
        private readonly IBorrowInterface _borrowInterface;

        public BorrowController(ISessionInterface sessionInterface, IBookInterface bookInterface,
            IBorrowInterface borrowInterface)
        {
            _sessionInterface = sessionInterface;
            _bookInterface = bookInterface;
            _borrowInterface = borrowInterface;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> Borrow(int id)
        {
            var sessionUser = _sessionInterface.FindSession();
            if (sessionUser == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to borrow books!";
                return RedirectToAction("Login", "Home");
            }

            var borrow = await _borrowInterface.Borrow(id);

            if (!borrow.Status)
            {
                TempData["ErrorMessage"] = borrow.Message;
                return RedirectToAction("Index", "Home");
            }

            TempData["SuccessMessage"] = "Borrowing completed successfully!";

            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public async Task<ActionResult> Return(int id)
        {
            var sessionUser = _sessionInterface.FindSession();
            if (sessionUser == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to return books!";
                return RedirectToAction("Login", "Home");
            }

            var borrow = await _borrowInterface.Return(id);

            TempData["SuccessMessage"] = "Book successfully returned!";
            return RedirectToAction("Index", "Home");
        }
    }
    
    
    
        
    
    
    }
