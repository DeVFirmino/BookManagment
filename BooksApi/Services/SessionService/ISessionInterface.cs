/*
------------------------------------------------------------
 SESSION INTERFACE (ISessionInterface)
------------------------------------------------------------
Purpose  
• Defines a contract for managing user sessions within the system.  
• Ensures consistent session handling across controllers and services.

Methods  
• FindSession()  
  → Retrieves the currently logged-in user from the session.  

• CreateSession(UserModel user)  
  → Stores user information in the session after successful login.  

• RemoveSession()  
  → Clears the current user session (used during logout).  

Usage  
• Implemented by `SessionService`.  
• Used in controllers like HomeController, BorrowController, etc.,
  to manage authentication state between requests.
------------------------------------------------------------
*/

using BooksApi.Models;

namespace BooksApi.Services.SessionService;

public interface ISessionInterface
{
    UserModel FindSession();
    void CreateSession(UserModel user);
    void RemoveSession();
}