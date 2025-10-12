/*
------------------------------------------------------------
 CLIENT CONTROLLER
------------------------------------------------------------
• Responsible for managing client-related operations and 
  interactions with borrowed books.

• Main functionalities:
  - Display the list of registered clients.
  - Allow clients to view their borrowing history.
  - Filter borrowed books by status (Returned / Not Returned).
  - Enable admin users to monitor client activity.

• Access control:
  - Requires user authentication through session validation.
  - Some actions restricted to administrators via filters.
------------------------------------------------------------
*/

using Microsoft.AspNetCore.Mvc;
using BooksApi.Services.UserService;
using System.Threading.Tasks;
using BooksApi.Filter;
using BooksApi.Services.BorrowService;
using BooksApi.Services.SessionService;

namespace BooksApi.Controllers;

[UserLogged]
public class ClientController : Controller
{
    private readonly IUserInterface _userInterface;
    private readonly ISessionInterface _sessionInterface;
    private readonly IBorrowInterface _borrowInterface;

    public ClientController(
        IUserInterface userInterface,
        ISessionInterface sessionInterface,
        IBorrowInterface borrowInterface)
    {
        _userInterface = userInterface;
        _sessionInterface = sessionInterface;
        _borrowInterface = borrowInterface;
    }

    public async Task<ActionResult> Index(int? id)
    {
        var clients = await _userInterface.FindUsers(id);
        return View(clients);
    }
    
    [UserLoggedAdmin]
    public async Task<ActionResult> Profile(string search = null, string filter = "NotReturned")
    {
        var sessionUser = _sessionInterface.FindSession();
        if (sessionUser == null)
            return RedirectToAction("Login", "Home");

        ViewBag.Filter = filter;

        if (search != null)
        {
            var filteredBorrows = await _borrowInterface.SearchBorrowsFilter(sessionUser, search);
            return View(filteredBorrows);
        }

        var userBorrows = await _borrowInterface.SearchBorrows(sessionUser);

        return View(userBorrows);
    }
}