/*
------------------------------------------------------------
 USER CONTROLLER
------------------------------------------------------------
Purpose
• Manages user CRUD-like flows: list, register, details, edit, and toggle status.

Endpoints
• GET  /User/Index?id={id}            -> Lists users by filter (id=0 clients, null employees, specific id)
• GET  /User/Register?id={id}         -> Shows create form (id != null => Client, else Admin)
• POST /User/Register                 -> Creates user (validates email/username uniqueness)
• GET  /User/Details/{id}             -> Shows user details
• POST /User/ToggleUserStatus         -> Activates/Deactivates a user by Id
• GET  /User/Edit/{id}                -> Loads edit form mapped to UserEditDto + AddressEditDto
• POST /User/Edit                     -> Persists edits; redirects by profile

Dependencies
• IUserInterface  – user persistence/queries and business rules
• IMapper         – maps domain models <-> DTOs (UserEditDto, AddressEditDto)

Access Control
• [UserLogged], [UserLoggedClient] on sensitive actions (Index/Details/Edit/Toggle).

User Feedback
• TempData["SuccessMessage"] / TempData["ErrorMessage"] to notify results.

Redirect Rules
• Clients redirect to Client/Index?id=0
• Non-clients (admins/employees) redirect to Employer/Index
------------------------------------------------------------
*/

using BooksApi.Dto.User;
using BooksApi.Enums;
using BooksApi.Models;
using BooksApi.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoMapper;
using BooksApi.Dto;
using BooksApi.Dto.Address; // AddressEditDto
using BooksApi.Dto.User;       // UserEditDto
using BooksApi.Enums;
using BooksApi.Filter; // ProfileEnum

namespace BooksApi.Controllers;


 
public class UserController : Controller
{
    private readonly IUserInterface _userInterface;
    private readonly IMapper _mapper;

    public UserController(IUserInterface userInterface, IMapper mapper)
    {
        _userInterface = userInterface;
        _mapper = mapper;
    }
    
    
    [UserLogged]
    [UserLoggedClient]
    public async Task<IActionResult> Index(int? id)
    {
        if (id == null) return RedirectToAction("Index");
        var user = await _userInterface.FindUsers(id.Value);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpGet]
    public ActionResult Register(int? id)
    {
        ViewBag.Profile = ProfileEnum.Administrator;
        ViewBag.Id = id;

        if (id != null)
        {
            ViewBag.Profile = ProfileEnum.Client;
        }

        return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> Register(UserCreationDto userCreationDto)
    {
        if (ModelState.IsValid)
        {
            if (!await _userInterface.VerifyIfUserAndEmailExist(userCreationDto))
            {
                TempData["ErrorMessage"] = "There is already an email/username registered!";
                return View(userCreationDto);
            }

            // Register user
            var user = await _userInterface.Register(userCreationDto);

            TempData["SuccessMessage"] = "Registration completed successfully!";

            if (user.Profile != ProfileEnum.Client)
            {
                return RedirectToAction("Index", "Employer");
            }

            return RedirectToAction("Index", "Client", new { Id = "0" });
        }

        return View(userCreationDto);
    }
    
    [HttpGet]
    [UserLogged]
    [UserLoggedClient]
    public async Task<ActionResult> Details(int? id)
    {
        if (id != null)
        {
            var user = await _userInterface.FindUsersById(id);
            return View(user);
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [UserLogged]
    [UserLoggedClient]
    public async Task<ActionResult> ToggleUserStatus(UserModel user)
    {
        if (user == null || user.Id <= 0)
            return RedirectToAction("Index");

        var updatedUser = await _userInterface.ToggleUserStatus(user.Id);

        if (updatedUser == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = updatedUser.IsActive
            ? "User activated successfully!"
            : "User deactivated successfully!";

        // Redirect depending on profile
        if (updatedUser.Profile == ProfileEnum.Client)
        {
            return RedirectToAction("Index", "Client", new { id = 0 });
        }

        return RedirectToAction("Index", "Employer");
    }
    
    
    [HttpGet]
    [UserLogged]
    [UserLoggedClient]
    public async Task<ActionResult> Edit(int? id)
    {
        if (id == null) return RedirectToAction("Index");

        var user = await _userInterface.FindUsersById(id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }

        var userEdit = new UserEditDto
        {
            Id        = user.Id,
            FullName  = user.FullName,
            Email     = user.Email,
            Profile   = user.Profile,
            Username  = user.Username,
            Address   = user.Address != null
                ? _mapper.Map<AddressEditDto>(user.Address)
                : new AddressEditDto()
        };

        // For the view to adjust labels/buttons
        ViewBag.Profile = (userEdit.Profile == ProfileEnum.Client)
            ? ProfileEnum.Client
            : ProfileEnum.Administrator;

        return View(userEdit); // Views/User/Edit.cshtml
    }
    
    
    [HttpPost]
    [UserLogged]
    [UserLoggedClient]
    public async Task<ActionResult> Edit(UserEditDto userEditDto)
    {
        if (ModelState.IsValid)
        {
            var user = await _userInterface.Edit(userEditDto);
            TempData["SuccessMessage"] = "Edit completed successfully!";

            if (user.Profile != ProfileEnum.Client)
            {
                return RedirectToAction("Index", "Employer");
            }
            else
            {
                return RedirectToAction("Index", "Client", new { Id = "0" });
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Please check the information provided!";
            return View(userEditDto);
        }
    }
    
     
    
    
}