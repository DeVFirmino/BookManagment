/*
------------------------------------------------------------
 AUTHENTICATION SERVICE
------------------------------------------------------------
Purpose  
• Handles secure password creation and validation for users.  
• Ensures password protection using industry-standard hashing.

Core Methods  
• CreatePasswordHash(password, out hash, out salt)  
  - Generates a random 16-byte salt.  
  - Hashes the password using PBKDF2 with SHA256, 100,000 iterations.  
  - Outputs both the hash and salt for database storage.  

• VerifyLogin(password, storedHash, storedSalt)  
  - Recreates the hash using the stored salt and compares it with the saved hash.  
  - Uses `CryptographicOperations.FixedTimeEquals` to prevent timing attacks.

Security Notes  
• PBKDF2 (Password-Based Key Derivation Function 2) ensures resistance to brute-force attacks.  
• Salt guarantees that identical passwords generate unique hashes.  
• This service is used in the login and registration processes for password handling.
------------------------------------------------------------
*/

using System.Security.Cryptography;

namespace BooksApi.Services.Authentication;

public class AuthenticationService : IAuthenticationInterface
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        // gera salt aleatório de 16 bytes
        passwordSalt = RandomNumberGenerator.GetBytes(16);

        // PBKDF2 com SHA256, 100.000 iterações, 32 bytes de saída
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, 100000, HashAlgorithmName.SHA256))
        {
            passwordHash = pbkdf2.GetBytes(32);
        }
    }

    public bool VerifyLogin(string password, byte[] storedHash, byte[] storedSalt)
    {
        // Recalculate the hash using the same parameters used when creating it
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 100000, HashAlgorithmName.SHA256))
        {
            var computedHash = pbkdf2.GetBytes(32);

            // Use a time-safe comparison to avoid timing attacks
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}