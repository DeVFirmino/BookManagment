using Microsoft.AspNetCore.Mvc;
using BooksApi.Dto;
using BooksApi.Models;
using BooksApi.Services.BookService;

namespace BooksApi.Controllers
{
    
   
    public class BookController : Controller
    {
        private readonly IBookInterface _bookInterface;

        public BookController(IBookInterface bookInterface)
        {
            _bookInterface = bookInterface;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookInterface.SearchBooks();
            return View(books);
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(BookCreationDto bookCreationDto, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                TempData["MErrorMessage"] = "Include the cover!";
                return View(bookCreationDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["MErrorMessage"] = "Please correct the highlighted fields.";
                return View(bookCreationDto);
            }

            if (_bookInterface.VerifyIfExists(bookCreationDto))
            {
                TempData["MErrorMessage"] = "Already registered!";
                return View(bookCreationDto);
            }

            var book = await _bookInterface.Register(bookCreationDto, photo);

            TempData["MSuccessMessage"] = "Book registered!";
            return RedirectToAction(nameof(Index));
        }
    }
}