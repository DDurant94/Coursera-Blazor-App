namespace BlazorApp.Models;

public class UserSession
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>True when an email address has been captured — i.e. the user has registered at least once.</summary>
    public bool IsActive => !string.IsNullOrEmpty(Email);
}
