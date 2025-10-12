/*
------------------------------------------------------------
 HOME SERVICE INTERFACE (IHomeInterface)
------------------------------------------------------------
Purpose  
• Defines the contract for login and authentication operations  
  handled by the HomeService.

Main Method  
• PerformLogin(LoginDto loginDto)  
  → Validates user credentials (email and password).  
  → Returns a ResponseModel<UserModel> indicating success or failure.

Usage  
• Implemented by `HomeService`.  
• Called by `HomeController` during the user login process.  
• Promotes abstraction and testability for authentication logic.
------------------------------------------------------------
*/


using BooksApi.Dto.Home;
using BooksApi.Models;
using System.Threading.Tasks;

namespace BooksApi.Services.HomeService;

public interface IHomeInterface
{
    Task<ResponseModel<UserModel>> PerformLogin(LoginDto loginDto);
}
 