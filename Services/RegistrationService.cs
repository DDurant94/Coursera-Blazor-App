using BlazorApp.Models;

namespace BlazorApp.Services;

public class RegistrationService : IRegistrationService
{
    private readonly List<Registration> _registrations = new();
    private int _nextId = 1;

    public List<Registration> GetRegistrationsForEvent(int eventId) =>
        _registrations.Where(r => r.EventId == eventId).ToList();

    // Counts without allocating a new list — used by high-frequency render paths.
    public int GetRegistrationCount(int eventId) =>
        _registrations.Count(r => r.EventId == eventId);

    public bool IsEventFull(int eventId, int capacity) =>
        GetRegistrationCount(eventId) >= capacity;

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
