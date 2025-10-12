/*
------------------------------------------------------------
 USER LOGGED CLIENT FILTER
------------------------------------------------------------
Purpose
• Restricts access for users with the Client profile.
• Ensures that certain pages (e.g., admin sections) are not accessible
  by regular clients.

Behavior
• If there is no active session → redirects to Home/Login.
• If the session exists but belongs to a Client → redirects to Home/Index.
• Grants access only to users who are not Clients (e.g., Administrators).

Usage
• Apply [UserLoggedClient] to controllers or actions that should be
  inaccessible to regular users.

Example
    [UserLoggedClient]
    public class EmployerController : Controller
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

namespace BooksApi.Filter
{
    public class UserLoggedClient : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userSession = context.HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Home"},
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
                        {"controller", "Home"},
                        {"action", "login"}
                    });
                }
                else if (user.Profile == ProfileEnum.Client)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"controller", "Home"},
                        {"action", "index"}
                    });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}