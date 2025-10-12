/*
------------------------------------------------------------
 USER EDIT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Handles data for updating existing users in the system.
• Separates editable fields from the core UserModel for security and structure.
• Includes address information via AddressEditDto for nested updates.

Fields
• Id        – Unique identifier of the user being edited.
• FullName  – Updated full name.
• Username  – Updated username.
• Email     – Updated email address.
• Profile   – User’s role (Administrator or Client).
• Address   – Nested object containing address fields (street, number, city, etc.).

Usage
• Used in UserController for editing user data.
• Ensures proper validation and prevents direct model manipulation.
• Keeps input validation separate from entity logic.
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;
using BooksApi.Enums;
using BooksApi.Dto;
using BooksApi.Dto.Address;

// /// <summary>
// /// DTO used to edit a user. Mirrors the editable fields from UserModel
// /// and groups address edits under AddressEditDto.  
// ///  
namespace BooksApi.Dto.User
{
    
    public class UserEditDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the full name!")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the username!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the email!")]
        [EmailAddress(ErrorMessage = "Enter a valid email!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select a profile!")]
        public ProfileEnum Profile { get; set; }

        [Required(ErrorMessage = "Provide the address info!")]
        public AddressEditDto Address { get; set; } = new();
    }
}