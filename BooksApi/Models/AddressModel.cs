/*
------------------------------------------------------------
 ADDRESS MODEL
------------------------------------------------------------
Purpose
• Represents the address entity linked to a user in the system.
• Stores location and contact-related information for each user.

Database Mapping
• Table Name → "Addresses"
• Primary Key → Id
• Foreign Key → UserId (references UserModel)

Main Properties
• Street, City, Number, ZipCode, State → Required address fields.
• AdditionalInfo → Optional extra details (e.g., apartment, reference).
• User → Navigation property (hidden in JSON to avoid circular references).

Usage
• Used by UserModel as a one-to-one relationship.
• Also used in DTOs (e.g., AddressEditDto) for form validation and editing.
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BooksApi.Models
{
    [Table("Addresses")]

    public class AddressModel
    {
        [Key] //dto
        public int Id { get; set; }

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        public string? AdditionalInfo { get; set; } = string.Empty;

        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel User { get; set; }
    }
}