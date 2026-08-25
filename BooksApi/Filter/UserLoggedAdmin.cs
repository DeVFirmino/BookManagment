/*
------------------------------------------------------------
 USER LOGGED ADMIN FILTER
------------------------------------------------------------
Purpose
• Ensures that only administrators can access specific controllers or actions.
• Validates user session and role before the request executes.

Behavior
• If no user session exists → redirects to Home/Login.
• If a user session exists but the profile is not Administrator → redirects to Home/Index.
• Allows access only when the user has ProfileEnum.Administrator.

Usage
• Apply [UserLoggedAdmin] on controllers or actions restricted to admins.
• Commonly used for areas like reports or user management.

Example
    [UserLoggedAdmin]
    public class ReportController : Controller
    {
        ...
    }
------------------------------------------------------------
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using BooksApi.Models;
using BooksApi.Enums;
namespace BooksApi.Filter;

public class UserLoggedAdmin : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string userSession = context.HttpContext.Session.GetString("UserSession");
        if (string.IsNullOrEmpty(userSession))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                {"controller", "home"},
                {"action", "login"}
            });
        }
        else
        {
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "home"},
                    {"action", "login"}
                });
            }
            else if (user.Profile != ProfileEnum.Administrator)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "home"},
                    {"action", "index"}
                });
            }
        }

        base.OnActionExecuting(context);
    }
}