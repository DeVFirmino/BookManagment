/*
------------------------------------------------------------
 HOME CONTROLLER
------------------------------------------------------------
Purpose
• Manages the main public-facing pages of the application,
  including home, login, logout, and book details.

Endpoints
• GET  /Home/Index?search={term}
  - Displays available books from the database.
  - Supports search filtering by title/author.
  - Fetches and displays a random motivational quote from a public API.

• GET  /Home/Login
  - Displays login page (redirects if already logged in).

• POST /Home/Login
  - Authenticates the user using HomeService.
  - Creates a session if successful and stores user data.
  - Handles inactive user accounts gracefully.

• GET  /Home/Logout
  - Ends user session and redirects to login page.

• GET  /Home/Details/{id}
  - Displays detailed information about a specific book.
  - Adjusts layout dynamically based on login status.
  - Handles borrow status (if user already borrowed or not).

Dependencies
• ISessionInterface – manages session creation/removal and retrieval.
• IHomeInterface – performs login validation and logic.
• IBookInterface – retrieves books for home display and details.
• Quote APIs (Quotable & DummyJSON) – fetches random inspirational quotes.

User Experience
• Uses dynamic layouts (_Layout / _LayoutLogout).
• Displays TempData messages for success/error feedback.
• Fallback quote displayed if APIs fail.

------------------------------------------------------------
*/

using BooksApi.Dto.Home;
using BooksApi.Enums;
using BooksApi.Models;
using BooksApi.Services.BookService;
using BooksApi.Services.HomeService;
using BooksApi.Services.SessionService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using BooksApi.Dto.Quote;


namespace BooksApi.Controllers;

public class HomeController : Controller
{
    public readonly ISessionInterface _sessionInterface;
    public readonly IHomeInterface _homeInterface;
    public readonly IBookInterface _bookInterface;
    public HomeController(ISessionInterface sessionInterface, IHomeInterface homeInterface, IBookInterface bookInterface)
    {
        _sessionInterface = sessionInterface;
        _homeInterface = homeInterface;
        _bookInterface = bookInterface;
 
    }
 
    [HttpGet]
    public async Task<ActionResult> Index(string search = null)
    {
        var userSession = _sessionInterface.FindSession();

        if (userSession != null)
            ViewBag.PageLayout = "_Layout";
        else
            ViewBag.PageLayout = "_LayoutLogout";

        QuoteDto? quote = null;

// 1) Primary: Quotable
        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
            http.DefaultRequestHeaders.UserAgent.ParseAdd("BooksApi/1.0 (+http://localhost)");
            quote = await http.GetFromJsonAsync<QuoteDto>("https://api.quotable.io/random?maxLength=120");
        }
        catch {   }

// 2) Backup: DummyJSON quotes (structure: { quote, author })
        if (quote == null || string.IsNullOrWhiteSpace(quote.content))
        {
            try
            {
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
                http.DefaultRequestHeaders.UserAgent.ParseAdd("BooksApi/1.0 (+http://localhost)");
                var backup = await http.GetFromJsonAsync<BackupQuoteDto>("https://dummyjson.com/quotes/random");
                if (backup != null && !string.IsNullOrWhiteSpace(backup.quote))
                {
                    quote = new QuoteDto { content = backup.quote, author = backup.author ?? "Unknown" };
                }
            }
            catch { }
        }

         ViewBag.Quote = quote ?? new QuoteDto
        {
            content = "Keep it simple. Ship it.",
            author  = "Unknown"
        };
  

        // 🔹 Search books 
        if (string.IsNullOrEmpty(search))
        {
            var booksFromDb = await _bookInterface.SearchBooks();
            return View(booksFromDb);
        }
        else
        {
            var filteredBooks = await _bookInterface.FindBooksByFilter(search);
            return View(filteredBooks);
        }
    }
    
    [HttpGet]
    public ActionResult Login()
    {
        if (_sessionInterface.FindSession() != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        if (ModelState.IsValid)
        {
            var login = await _homeInterface.PerformLogin(loginDto);

            if (login.Status == false)
            {
                TempData["ErrorMessage"] = login.Message;
                return View(login.Data); // keep the user inputs (email)
            }

            if (login.Data.IsActive == false)
            {
                TempData["ErrorMessage"] = "Please contact support to verify the status of your account!";
                return View("Login");
            }

            _sessionInterface.CreateSession(login.Data);
            TempData["SuccessMessage"] = login.Message;

            return RedirectToAction("Index", "Home");
        }

        return View(loginDto);
    }
    
    [HttpGet]
    public ActionResult Logout()
    {
        _sessionInterface.RemoveSession();
        TempData["SuccessMessage"] = "User logged out!";
        return RedirectToAction("Login", "Home");
    }
    
    [HttpGet]
    public async Task<ActionResult> Details(int? id)
    {
        var sessionUser = _sessionInterface.FindSession();

        if (sessionUser != null)
        {
            ViewBag.LoggedUser = sessionUser.Id;
            ViewBag.LayoutPage = "_Layout";
        }
        else
        {
            ViewBag.LayoutPage = "_LayoutLogout";
        }

        var book = await _bookInterface.SearchBookById(id, sessionUser);

        if (book.User != null)
        {
            if (book.User.Borrow == null)
            {
                ViewBag.Borrow = "NoBorrow";
            }
        }

        return View(book);
    }

       
    
}
