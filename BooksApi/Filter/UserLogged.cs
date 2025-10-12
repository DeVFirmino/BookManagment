/// <summary>
/// Action filter that ensures the user is authenticated before accessing a controller or action.
/// If there is no active session (UserSession not found), it redirects to Home/Login.
/// Use this attribute on controllers or actions that require login verification.
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using BooksApi.Models;

namespace BooksApi.Filter
{
    public class UserLogged : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userSession = context.HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Home"},
                    {"action", "Login"}
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
                        {"action", "Login"}
                    });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}