# EventEase — Event Management Application

**Author:** Daniel Durant

## What is this?

EventEase is a corporate event management app built with Blazor WebAssembly. Users can browse upcoming events, get the full details on anything that interests them, and register — all without a page reload. As registrations come in, each event card updates its availability in real time so nobody gets a nasty surprise when they try to sign up for something that's already full.

## What can it do?

- **Browse events** — A paginated grid of event cards shows the name, date, location, category, and how many spots are still open. Six cards per page keeps things readable without endless scrolling.
- **View event details** — Click any card to get the full picture: description, time, location, and a live progress bar that turns yellow when seats are getting scarce and red when the event is full.
- **Register** — A clean form with real-time validation. It won't let you register twice with the same email, and if the last seat fills up while you're typing, it catches that too.
- **Graceful errors** — Navigate to a URL that doesn't exist and you'll land on a proper 404 page instead of a blank screen, with quick links to get back on track.
- **Landing page** — The home page gives new visitors a quick overview of what EventEase is and a peek at three upcoming events to draw them in.

## How it's built

### Pages and components

| Component | URL | What it does |
|---|---|---|
| `Home.razor` | `/` | Landing page — hero section and event preview |
| `Events.razor` | `/events` | The main event listing, paginated |
| `EventDetail.razor` | `/events/{id:int}` | Full details for a single event |
| `EventRegistration.razor` | `/events/{id:int}/register` | Registration form |
| `NotFound.razor` | anything unknown | 404 page |

### Services

Two singleton services hold the app's state for the lifetime of the browser session:

- **`EventService`** — owns the list of 9 sample events
- **`RegistrationService`** — stores registrations and handles capacity checks, duplicate detection, and registration counts

Because they're singletons, navigating between pages doesn't reset anything — register for an event on the form page, go back to the listing, and the availability bar will already reflect your registration.

### Two-way data binding

The registration form uses Blazor's `@bind-Value` on every field, so the `Registration` model stays in sync with what the user is typing without any manual event handling. When the form is submitted, `DataAnnotationsValidator` validates that bound model against the data annotation attributes on the model class — if anything fails, the relevant error messages appear inline next to the offending field before `HandleValidSubmit` ever runs.

### Keeping it fast

A few deliberate choices to avoid unnecessary work on every render:

- Availability stats (registered count, spots left, fill percentage, bar colour) are computed **once** when the page loads and stored in a `Dictionary`. The render loop just reads from that dictionary — no service calls, no list allocations per card per render.
- Registration counts use `Count(predicate)` directly on the list rather than `Where(...).ToList().Count`, which would allocate a new list just to count it.
- Pagination keeps the Blazor component tree small — only the six cards on the current page exist in the diff at any given time.

### Validation

The `Registration` model is annotated with:

- `[Required]` and `[MaxLength]` on every field so nothing sneaks through blank or oversized
- `[EmailAddress]` to catch malformed email addresses
- `[Phone]` plus a `[RegularExpression]` stacked on top of it for the phone field — the built-in `[Phone]` attribute alone accepts letters, so the regex tightens it to digits, spaces, dashes, dots, and parentheses in a sensible length range
- HTML `maxlength` attributes on every input to match the model constraints, so the browser itself pushes back before a character even reaches the validator

### Routing and edge cases

A few guard rails that make the app behave predictably under unexpected input:

- The route `{EventId:int}` means a URL like `/events/abc` is rejected by the router before any component code runs
- Negative or zero IDs are caught in `OnParametersSet` and redirect back to `/events` — with `replace: true` so the bad URL doesn't sit in the browser's history
- Capacity is re-checked at the moment the form is submitted, not just when the page loads, to handle the case where the last seat is taken by someone else while the form is open
- Unknown routes hit the `<NotFound>` slot in `App.razor`, which renders the 404 page inside the normal layout rather than a blank white screen

### A note on security

Blazor renders `@variable` output as text nodes, not raw HTML, so user-supplied strings can't inject markup or script. The duplicate-registration check uses case-insensitive email comparison so `User@Example.COM` and `user@example.com` are treated as the same address.

## Stack

| | |
|---|---|
| Framework | Blazor WebAssembly (.NET 10) |
| Language | C# |
| Styling | Bootstrap 5 |
| Validation | `System.ComponentModel.DataAnnotations` |
| State | In-memory singleton services |
| Routing | Blazor Router with typed route constraints |

## Academic context

This is the capstone project for **Module Four: Blazor for Front-End Development** in the [Microsoft Full-Stack Developer Professional Certificate](https://www.coursera.org/professional-certificates/microsoft-full-stack-developer) on Coursera. The module covers Blazor's component model, two-way data binding, routing, and form validation — all of which are demonstrated here. GitHub Copilot was used throughout the development process to explore patterns, generate boilerplate, and speed up iteration.
