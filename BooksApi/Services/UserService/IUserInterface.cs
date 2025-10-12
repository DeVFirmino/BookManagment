// This interface defines the contract for User-related operations.  
// It ensures that any service implementing it will provide:  
//   - FindUsers(int? id): Retrieves a list of users, optionally filtered by an id/profile.  
// 
// (Other methods like GetUserByIdAsync and GetAllUsersAsync are currently commented out,  
// but can be re-enabled to expand functionality.)  
// 
// Purpose: To separate the definition of User service methods from their implementation,  
// promoting clean architecture, testability, and dependency injection.

using BooksApi.Dto.User;
using BooksApi.Models;

namespace BooksApi.Services.UserService
{
    public interface IUserInterface
    {

        Task<List<UserModel>> FindUsers(int? id);
        Task<bool> VerifyIfUserAndEmailExist(UserCreationDto _userCreationDto);

        Task<UserCreationDto> Register(UserCreationDto _userCreationDto);

        Task<UserModel> FindUsersById(int? id);
        //     
        //     Task<List<UserModel>> GetAllUsersAsync(int? id);
        
        Task<UserModel?> ToggleUserStatus(int id);

        Task<UserModel> Edit(UserEditDto userEditDto);
    }
}