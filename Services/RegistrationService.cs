using BlazorApp.Models;

namespace BlazorApp.Services;

public class RegistrationService : IRegistrationService
{
    private readonly List<Registration> _registrations = new();
    private int _nextId = 1;

    public List<Registration> GetRegistrationsForEvent(int eventId) =>
        _registrations.Where(r => r.EventId == eventId).ToList();

    public void RegisterForEvent(Registration registration)
    {
        registration.Id = _nextId++;
        registration.RegisteredOn = DateTime.Now;
        _registrations.Add(registration);
    }

    public bool IsAlreadyRegistered(int eventId, string email) =>
        _registrations.Any(r => r.EventId == eventId &&
            r.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
}
