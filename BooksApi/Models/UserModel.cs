/*
------------------------------------------------------------
 USER MODEL
------------------------------------------------------------
Purpose  
• Represents the user entity stored in the database.  
• Contains authentication, profile, and address information.  
• Serves as the main link between users and their book borrowings.

Database Mapping  
• Table → "Users"  
• Primary Key → Id  
• Relationships:  
   - Address → One-to-one (User has one AddressModel).  
   - Borrow  → One-to-many (User can have multiple BorrowModel records).

Main Properties  
• FullName / Username / Email → Core identification fields.  
• IsActive → Defines whether the user account is active.  
• Profile → User role, defined by ProfileEnum (Administrator or Client).  
• PasswordHash / PasswordSalt → Secure storage for hashed credentials.  
• CreatedAt / UpdatedAt → Track record creation and modification timestamps.

Usage  
• Used across authentication, authorization, and reporting modules.  
• Accessed in controllers (UserController, HomeController) and filters for session management.
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BooksApi.Enums;

namespace BooksApi.Models
{
    [Table("Users")]

    public class UserModel
    {
        [Key]
        public int Id{ get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public ProfileEnum Profile { get; set; }

        // [Required]
        // public ShiftEnum Shift { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public AddressModel Address { get; set; }

        public List<BorrowModel> Borrow { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}