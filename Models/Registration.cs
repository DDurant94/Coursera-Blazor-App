using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [RegularExpression(@"^\+?[\d\s\-\(\)\.]{7,20}$",
        ErrorMessage = "Please enter a valid phone number (e.g. (555) 123-4567 or +1 555-123-4567).")]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime RegisteredOn { get; set; } = DateTime.Now;
}
