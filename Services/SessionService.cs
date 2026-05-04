using System.Text.Json;
using BlazorApp.Models;
using Microsoft.JSInterop;

namespace BlazorApp.Services;

public class SessionService : ISessionService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "eventease_session";

    public SessionService(IJSRuntime js)
    {
        _js = js;
    }

    public UserSession CurrentUser { get; private set; } = new();

    public bool HasSession => CurrentUser.IsActive;

    public event Action? OnSessionChanged;

    /// <summary>Called once at app startup to restore a previous session from localStorage.</summary>
    public async Task InitializeAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
            {
                var saved = JsonSerializer.Deserialize<UserSession>(json);
                if (saved?.IsActive == true)
                {
                    CurrentUser = saved;
                    OnSessionChanged?.Invoke();
                }
            }
        }
        catch
        {
            // localStorage may be unavailable in restricted browser contexts; fail silently.
        }
    }

    public void StartSession(string firstName, string lastName, string email, string phoneNumber)
    {
        CurrentUser = new UserSession
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber
        };
        _ = PersistAsync();
        OnSessionChanged?.Invoke();
    }

    public void ClearSession()
    {
        CurrentUser = new();
        _ = RemoveAsync();
        OnSessionChanged?.Invoke();
    }

    private async Task PersistAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(CurrentUser);
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch { }
    }

    private async Task RemoveAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        }
        catch { }
    }
}
