using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IEventService
{
    List<Event> GetAllEvents();
    Event? GetEventById(int id);
}
