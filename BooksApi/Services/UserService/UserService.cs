/*
------------------------------------------------------------
 USER SERVICE (UserService)
------------------------------------------------------------
Purpose
• Business/service layer for user management.
• Handles querying, creation, edition, status toggle, and lookups.

Key Dependencies
• AppDbContext                → EF Core data access.
• IAuthenticationInterface    → Password hashing & verification.
• IMapper (AutoMapper)        → Maps DTOs ⇄ Entities (e.g., AddressEditDto ↔ AddressModel).

Main Methods
• FindUsers(int? profile)
  → Returns users (optionally filtered by Profile).

• VerifyIfUserAndEmailExist(UserCreationDto dto)
  → Checks if email or username already exists (returns true when available).

• Register(UserCreationDto dto)
  → Creates User + Address, hashes password (PBKDF2), saves to DB.

• FindUsersById(int? id)
  → Loads a single user with Address.

• ToggleUserStatus(int id)
  → Flips IsActive and updates timestamp.

• Edit(UserEditDto dto)
  → Updates basic fields and nested address via AutoMapper.

Notes
• All DB calls are async (EF Core).
• Exceptions are bubbled up with message preservation.
------------------------------------------------------------
*/
using AutoMapper;
using BooksApi.Data;
using BooksApi.Dto.User;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using BooksApi.Services.Authentication;


namespace BooksApi.Services.UserService;

public class UserService : IUserInterface
{
    private readonly AppDbContext _context;
    private readonly IAuthenticationInterface _authenticationInterface;
    private readonly IMapper _mapper;


    public UserService(AppDbContext context, IAuthenticationInterface authenticationInterface, IMapper mapper)
    {
        _context = context;
        _authenticationInterface = authenticationInterface;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> FindUsers(int? profile)
    {
        try
        {
            var query = _context.Users
                .Include(u => u.Address)
                .AsQueryable();

            if (profile.HasValue)
            {
                query = query.Where(u => (int)u.Profile == profile.Value);
            }

            var registers = await query.ToListAsync();
            return registers;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> VerifyIfUserAndEmailExist(UserCreationDto userCreationDto)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(user =>
                user.Email == userCreationDto.Email || user.Username == userCreationDto.Username);

            if (existingUser == null)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserCreationDto> Register(UserCreationDto userCreationDto)
    {
        try
        {
            // Generate password hash and salt
            _authenticationInterface.CreatePasswordHash(userCreationDto.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new UserModel
            {
                FullName = userCreationDto.FullName,
                Username = userCreationDto.Username,
                Email = userCreationDto.Email,
                Profile = userCreationDto.Profile,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var address = new AddressModel
            {
                Street = userCreationDto.Street,
                Number = userCreationDto.Number,
                City = userCreationDto.City,
                State = userCreationDto.State,
                ZipCode = userCreationDto.ZipCode,
                AdditionalInfo = userCreationDto.AdditionalInfo,
                User = user
            };

            user.Address = address;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return userCreationDto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserModel> FindUsersById(int? id)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(dbUser => dbUser.Id == id);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserModel?> ToggleUserStatus(int id)
    {
        try
        {
            var userToUpdate = await _context.Users
                .FirstOrDefaultAsync(userDb => userDb.Id == id);

            if (userToUpdate != null)
            {
                // Toggle status
                userToUpdate.IsActive = !userToUpdate.IsActive;
                userToUpdate.UpdatedAt = DateTime.Now;

                _context.Update(userToUpdate);
                await _context.SaveChangesAsync();

                return userToUpdate;
            }

            return userToUpdate;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserModel> Edit(UserEditDto userEditDto)
{
    try
    {
        var userFromDb = await _context.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == userEditDto.Id);

        if (userFromDb != null)
        {
            userFromDb.Profile   = userEditDto.Profile;
            userFromDb.FullName  = userEditDto.FullName;
            userFromDb.Username  = userEditDto.Username;
            userFromDb.Email     = userEditDto.Email;
            userFromDb.UpdatedAt = DateTime.Now;

            // Use AutoMapper to map the AddressEditDto → AddressModel
            userFromDb.Address = _mapper.Map<AddressModel>(userEditDto.Address);

            // Make sure relationship is kept
            userFromDb.Address.UserId = userFromDb.Id;

            _context.Update(userFromDb);
            await _context.SaveChangesAsync();

            return userFromDb;
        }

        return null;
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
    }
