using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IRegistrationService
{
    List<Registration> GetAllRegistrations();
    List<Registration> GetRegistrationsForEvent(int eventId);
    int GetRegistrationCount(int eventId);
    bool IsEventFull(int eventId, int capacity);
    void RegisterForEvent(Registration registration);
    bool IsAlreadyRegistered(int eventId, string email);
    Registration? FindByEmail(string email);
}
