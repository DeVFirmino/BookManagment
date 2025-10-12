/*
------------------------------------------------------------
 SESSION SERVICE (SessionService)
------------------------------------------------------------
Purpose  
• Handles all session-related logic for storing and retrieving  
  user information across requests in the application.

How it works  
• Uses `IHttpContextAccessor` to access the current HTTP context  
  and manage session data.

Main Methods  
• FindSession()  
  → Retrieves the logged-in user from the session (if exists).  

• CreateSession(UserModel user)  
  → Serializes the user object to JSON and saves it in session storage.  

• RemoveSession()  
  → Deletes the "UserSession" entry from the session to log out the user.

Usage  
• Implements `ISessionInterface`.  
• Used by controllers (e.g., HomeController, BorrowController)  
  to maintain login persistence between requests.
------------------------------------------------------------
*/


using BooksApi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BooksApi.Services.SessionService;

public class SessionService : ISessionInterface
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserModel FindSession()
    {
        string? userSession = _httpContextAccessor.HttpContext?.Session.GetString("UserSession");

        if (string.IsNullOrEmpty(userSession))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<UserModel>(userSession);
    }

    public void CreateSession(UserModel user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        _httpContextAccessor.HttpContext?.Session.SetString("UserSession", userJson);
    }

    public void RemoveSession()
    {
        _httpContextAccessor.HttpContext?.Session.Remove("UserSession");
    }
}