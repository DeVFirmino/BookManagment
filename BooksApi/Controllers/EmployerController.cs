using BooksApi.Filter;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Services.UserService;

/*
------------------------------------------------------------
 EMPLOYER CONTROLLER
------------------------------------------------------------
• Responsible for managing and displaying the list of
  employees (administrative users) in the system.

• Main functionalities:
  - Retrieve and display all registered employees.
  - Provide access to administrative-level user data.

• Access control:
  - Requires user authentication.
  - Restricted to non-client (employee/admin) users through filters.
------------------------------------------------------------
*/

namespace BooksApi.Controllers;

[UserLogged]
[UserLoggedClient]
public class EmployerController : Controller
{
    private readonly IUserInterface _userInterface;

    public EmployerController( IUserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    // GET
    public async Task<ActionResult> Index()
    {
        var employers = await _userInterface.FindUsers(null);
        return View(employers);
    }
}