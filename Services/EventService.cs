using BlazorApp.Models;

namespace BlazorApp.Services;

public class EventService : IEventService
{
    private readonly List<Event> _events = new()
    {
        new Event
        {
            Id = 1,
            Name = "Annual Corporate Gala",
            Date = new DateTime(2026, 6, 15, 18, 0, 0),
            Location = "The Grand Ballroom, New York, NY",
            Description = "Join us for an elegant evening celebrating another successful year. Enjoy fine dining, live entertainment, and networking with industry leaders.",
            Capacity = 300,
            Category = "Corporate"
        },
        new Event
        {
            Id = 2,
            Name = "Tech Innovation Summit",
            Date = new DateTime(2026, 7, 22, 9, 0, 0),
            Location = "Convention Center, San Francisco, CA",
            Description = "A full-day summit featuring keynote speakers, panel discussions, and workshops focused on the latest trends in technology and innovation.",
            Capacity = 500,
            Category = "Conference"
        },
        new Event
        {
            Id = 3,
            Name = "Summer Charity Fundraiser",
            Date = new DateTime(2026, 8, 10, 17, 0, 0),
            Location = "Rooftop Terrace, Chicago, IL",
            Description = "An outdoor fundraising event benefiting local community programs. Features live music, silent auction, and refreshments with a beautiful city skyline view.",
            Capacity = 150,
            Category = "Charity"
        },
        new Event
        {
            Id = 4,
            Name = "Leadership Workshop Series",
            Date = new DateTime(2026, 9, 5, 8, 30, 0),
            Location = "EventEase Training Center, Austin, TX",
            Description = "An intensive half-day workshop designed to develop leadership skills for mid-level managers. Includes group exercises, case studies, and coaching sessions.",
            Capacity = 50,
            Category = "Workshop"
        },
        new Event
        {
            Id = 5,
            Name = "Holiday Networking Mixer",
            Date = new DateTime(2026, 12, 12, 19, 0, 0),
            Location = "The Skyline Lounge, Seattle, WA",
            Description = "End the year in style at our annual holiday networking mixer. Connect with professionals from various industries while enjoying seasonal cocktails and appetizers.",
            Capacity = 200,
            Category = "Networking"
        }
    };

    public List<Event> GetAllEvents() => _events;

    public Event? GetEventById(int id) => _events.FirstOrDefault(e => e.Id == id);
}
