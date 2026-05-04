using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IRegistrationService
{
    List<Registration> GetRegistrationsForEvent(int eventId);
    void RegisterForEvent(Registration registration);
    bool IsAlreadyRegistered(int eventId, string email);
}
