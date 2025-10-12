/*
------------------------------------------------------------
 HOME SERVICE (HomeService)
------------------------------------------------------------
Purpose  
• Handles user authentication and login validation logic.  
• Interacts directly with the database and authentication service.

Dependencies  
• AppDbContext → Accesses the Users table to find credentials.  
• IAuthenticationInterface → Verifies password hashes securely.

Key Method  
• PerformLogin(LoginDto loginDto)
  → Validates user credentials against the database.  
  → Uses the authentication service to verify password hash & salt.  
  → Returns a ResponseModel<UserModel> with login result details.

Flow  
1. Looks up the user by email.  
2. If not found → returns “Invalid credentials”.  
3. If found → verifies password using PBKDF2 hashing.  
4. On success → returns user data and success message.  

Usage  
• Called by `HomeController.Login()` to authenticate users.  
• Ensures consistent and secure login handling.
------------------------------------------------------------
*/

using BooksApi.Data;
using BooksApi.Dto.Home;
using BooksApi.Models;
using BooksApi.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using BooksApi.Services.HomeService;


namespace BooksApi.Services.HomeService
{
    public class HomeService : IHomeInterface
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticationInterface _authenticationInterface;

        public HomeService(AppDbContext context, IAuthenticationInterface authenticationInterface)
        {
            _context = context;
            _authenticationInterface = authenticationInterface;
        }

        public async Task<ResponseModel<UserModel>> PerformLogin(LoginDto loginDto)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(dbUser => dbUser.Email == loginDto.Email);

                if (user == null)
                {
                    response.Data = null;
                    response.Message = "Invalid credentials";
                    response.Status = false;
                    return response;
                }

                if (!_authenticationInterface.VerifyLogin(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Data = null;
                    response.Message = "Invalid credentials";
                    response.Status = false;
                    return response;
                }

                response.Data = user;
                response.Message = "Login successfully completed!";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}