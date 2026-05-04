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
        },
        new Event
        {
            Id = 6,
            Name = "Product Launch Showcase",
            Date = new DateTime(2026, 10, 1, 10, 0, 0),
            Location = "EventEase Innovation Hub, Boston, MA",
            Description = "Be the first to experience our latest product lineup. This exclusive showcase features live demos, Q&A sessions with the product team, and early-access sign-ups.",
            Capacity = 120,
            Category = "Corporate"
        },
        new Event
        {
            Id = 7,
            Name = "Women in Tech Symposium",
            Date = new DateTime(2026, 10, 20, 9, 0, 0),
            Location = "Downtown Conference Center, Denver, CO",
            Description = "A full-day symposium celebrating and empowering women in technology. Features keynote speakers, mentorship roundtables, and a networking reception.",
            Capacity = 250,
            Category = "Conference"
        },
        new Event
        {
            Id = 8,
            Name = "Fall Team Building Retreat",
            Date = new DateTime(2026, 11, 7, 8, 0, 0),
            Location = "Blue Ridge Mountain Lodge, Asheville, NC",
            Description = "A two-day off-site retreat designed to strengthen team cohesion through collaborative activities, outdoor challenges, and strategic planning sessions.",
            Capacity = 75,
            Category = "Workshop"
        },
        new Event
        {
            Id = 9,
            Name = "Startup Pitch Night",
            Date = new DateTime(2026, 11, 19, 18, 30, 0),
            Location = "The Innovation Loft, Portland, OR",
            Description = "Watch emerging startups pitch their ideas to a panel of investors and industry experts. Open networking session and refreshments follow the presentations.",
            Capacity = 180,
            Category = "Networking"
        }
    };

    public List<Event> GetAllEvents() => _events;

    public Event? GetEventById(int id) => _events.FirstOrDefault(e => e.Id == id);
}
