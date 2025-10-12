/*
------------------------------------------------------------
 MENU VIEW COMPONENT (Menu)
------------------------------------------------------------
Purpose  
• Dynamically renders the navigation menu based on the current  
  logged-in user's session data.

How it works  
• Retrieves the "UserSession" JSON string from session storage.  
• If no session exists → returns the default (logged-out) menu.  
• If a session exists → deserializes `UserModel` and passes it  
  to the View for personalized display (e.g., username, role).

Usage  
• Invoked inside _Layout.cshtml via:
    @await Component.InvokeAsync("Menu")
• Provides dynamic top-bar behavior for authenticated users.
------------------------------------------------------------
*/

using System.Net;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace BooksApi.ViewComponents;


public class Menu : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string userSession = HttpContext.Session.GetString("UserSession");

        if (string.IsNullOrEmpty(userSession)) return View();

        UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);
        return View(user);
    }
}
