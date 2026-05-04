using BlazorApp.Models;

namespace BlazorApp.Services;

public interface ISessionService
{
    /// <summary>Restores the session from browser localStorage. Call once on app startup.</summary>
    Task InitializeAsync();

    /// <summary>The currently active user. Check <see cref="HasSession"/> before reading.</summary>
    UserSession CurrentUser { get; }

    /// <summary>True when the user has completed at least one registration this browser session.</summary>
    bool HasSession { get; }

    /// <summary>Persists the user's details after a successful registration.</summary>
    void StartSession(string firstName, string lastName, string email, string phoneNumber);

    /// <summary>Clears the active session (sign out).</summary>
    void ClearSession();

    /// <summary>Fired whenever the session starts or ends so subscribed components can update their UI.</summary>
    event Action? OnSessionChanged;
}
