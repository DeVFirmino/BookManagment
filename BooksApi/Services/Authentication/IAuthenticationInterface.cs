namespace BooksApi.Services.Authentication;

// Authentication contract (cross‑platform)
// ---------------------------------------------------------
// Purpose: Defines how password hashes are created and verified
// without tying the app to any OS-specific crypto APIs.
// Recommended implementation: PBKDF2 or HMACSHA512 (both work on macOS/Windows/Linux).
// Methods:
// - CreatePasswordHash: generates hash+salt from a plain password.
// - VerifyPassword: checks a plain password against a stored hash+salt.
// ---------------------------------------------------------
public interface IAuthenticationInterface
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyLogin(string password, byte[] storedHash, byte[] storedSalt);
}