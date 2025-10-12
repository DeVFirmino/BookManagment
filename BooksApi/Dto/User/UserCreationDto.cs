/*
------------------------------------------------------------
 USER CREATION DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Defines the data structure required to register a new user.
• Transfers user registration data from the View to the Controller/Service layer.
• Ensures input validation before saving to the database.
 

Usage
• Used in UserController to validate and register new users.
• Helps maintain separation between domain models and presentation logic.
------------------------------------------------------------
*/

using BooksApi.Enums;
using System.ComponentModel.DataAnnotations;


namespace BooksApi.Dto.User
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "Please enter the full name!")]
        public string FullName { get; set; } 

        [Required(ErrorMessage = "Please enter the username!")]
        public string Username { get; set; } 

        [Required(ErrorMessage = "Please enter the email!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Please select the profile!")]
        public ProfileEnum Profile { get; set; }

        // [Required(ErrorMessage = "Please select the shift!")]
        // public ShiftEnum Shift { get; set; }

        [Required(ErrorMessage = "Please enter the street!")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please enter the city!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter the number!")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Please enter the ZIP code!")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter the state!")]
        public string State { get; set; }

        public string? AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Please enter a password!")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long!")]
        public string Password { get; set; } 

        [Required(ErrorMessage = "Please confirm the password!")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
    }
}