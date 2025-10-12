/*
------------------------------------------------------------
 ADDRESS EDIT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents the data structure used to update or edit a user's address.
• Ensures only necessary and validated fields are sent between layers.

Validation
• Includes data annotations ([Required]) for essential fields.
• Provides clear error messages for form validation in the view layer.

Usage
• Used in User Edit workflows (UserEditDto) for updating address info.
------------------------------------------------------------
*/

using System.ComponentModel.DataAnnotations;

namespace BooksApi.Dto.Address;

public class AddressEditDto
{
    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Number is required.")]
    public string Number { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    public string State { get; set; }

    [Required(ErrorMessage = "ZipCode is required.")]
    public string ZipCode { get; set; }

    public string? AdditionalInfo { get; set; }

    [Required(ErrorMessage = "UserId is required.")]
    public int UserId { get; set; }
}