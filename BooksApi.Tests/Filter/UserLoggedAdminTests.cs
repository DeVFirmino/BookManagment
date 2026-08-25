using BooksApi.Enums;
using BooksApi.Filter;
using BooksApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace BooksApi.Tests.Filter;

public class UserLoggedAdminTests
{
    [Fact]
    public void OnActionExecuting_AllowsAccess_WhenUserIsAdministrator()
    {
        var context = CreateContext(SessionFor(ProfileEnum.Administrator));

        new UserLoggedAdmin().OnActionExecuting(context);

        Assert.Null(context.Result);
    }

    [Fact]
    public void OnActionExecuting_RedirectsToHome_WhenUserIsNotAdministrator()
    {
        var context = CreateContext(SessionFor(ProfileEnum.Client));

        new UserLoggedAdmin().OnActionExecuting(context);

        var redirect = Assert.IsType<RedirectToRouteResult>(context.Result);
        Assert.Equal("home", (string?)redirect.RouteValues?["controller"]);
        Assert.Equal("index", (string?)redirect.RouteValues?["action"]);
    }

    [Fact]
    public void OnActionExecuting_RedirectsToLogin_WhenNoSessionExists()
    {
        var context = CreateContext(sessionJson: null);

        new UserLoggedAdmin().OnActionExecuting(context);

        var redirect = Assert.IsType<RedirectToRouteResult>(context.Result);
        Assert.Equal("home", (string?)redirect.RouteValues?["controller"]);
        Assert.Equal("login", (string?)redirect.RouteValues?["action"]);
    }

    private static string SessionFor(ProfileEnum profile) =>
        JsonConvert.SerializeObject(new UserModel
        {
            Id = 1,
            FullName = "Test User",
            Username = "test.user",
            Email = "test@example.com",
            Profile = profile
        });

    private static ActionExecutingContext CreateContext(string? sessionJson)
    {
        var httpContext = new DefaultHttpContext { Session = new TestSession() };
        if (sessionJson != null)
        {
            httpContext.Session.SetString("UserSession", sessionJson);
        }

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
    }
}
