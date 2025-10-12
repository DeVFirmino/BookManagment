/*
------------------------------------------------------------
 LOGIN DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Defines the structure of the data required for user authentication.
• Ensures that login credentials are validated before processing.
• Used to safely transfer login information from the view to the controller.

Validation
• [Required] – Ensures that both email and password are provided.
• [EmailAddress] – Validates proper email format.
• [DataType(DataType.Password)] – Masks the password input field in forms.

Fields
• Email     – The user’s email address used for login.
• Password  – The corresponding password for authentication.

Usage
• Used in HomeController (Login actions) to authenticate users.
• Validates user input on the login form before attempting authentication.
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;

namespace BooksApi.Dto.Home
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Please enter your email!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}