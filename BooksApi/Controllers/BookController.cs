/*
------------------------------------------------------------
 BOOK CONTROLLER
------------------------------------------------------------
• Responsible for handling all book-related operations 
  within the application.

• Main functionalities:
  - Display the list of books available in the system.
  - Allow administrators to register new books with cover images.
  - Edit or update existing book information.
  - Display book details individually.

• Access control:
  - Restricted to logged users and clients via filters.
------------------------------------------------------------
*/

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Dto;
using BooksApi.Filter;
using BooksApi.Models;
using BooksApi.Services.BookService;

namespace BooksApi.Controllers
{

        [UserLogged]
        [UserLoggedClient]
    public class BookController : Controller
    {
        private readonly IBookInterface _bookInterface;
        public readonly IMapper _mapper;

        public BookController(IBookInterface bookInterface, IMapper mapper)
        {
            _bookInterface = bookInterface;
            _mapper = mapper;
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

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id != null)
            {
                var book = await _bookInterface.SearchBookById(id);
                return View(book);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var book = await _bookInterface.SearchBookById(id);
                var bookEditDto = _mapper.Map<BookEditDto>(book);

                return View(bookEditDto);
            }

            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<ActionResult> Edit(BookEditDto bookEditDto, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {

                var livro = await _bookInterface.Edit(bookEditDto, photo);
                return RedirectToAction("Index");

            }
            else
            {
                TempData["MErrorMessage"] = "Verify the form filled!";
                return View(bookEditDto);
            }
        }
    }
}