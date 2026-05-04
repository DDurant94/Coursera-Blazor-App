# EventEase — Event Management Application

**Author:** Daniel Durant

## What is this?

EventEase is a corporate event management app built with Blazor WebAssembly. Users can browse upcoming events, get the full details on anything that interests them, and register — all without a page reload. As registrations come in, each event card updates its availability in real time so nobody gets a nasty surprise when they try to sign up for something that's already full.

## What can it do?

- **Browse events** — A paginated grid of event cards shows the name, date, location, category, and how many spots are still open. Six cards per page keeps things readable without endless scrolling.
- **View event details** — Click any card to get the full picture: description, time, location, and a live progress bar that turns yellow when seats are getting scarce and red when the event is full.
- **Register** — A clean form with real-time validation. It won't let you register twice with the same email, and if the last seat fills up while you're typing, it catches that too.
- **Session tracking** — First time you register, EventEase remembers who you are for the rest of the session. Subsequent registration forms pre-fill your name, email, and phone number automatically.
- **My Registrations** — A personal attendance tracker listing every event you've signed up for, with upcoming events highlighted in green and past ones in dark. Session state survives a page refresh via `localStorage`.
- **Sign In** — Returning users (or anyone who signed out) can restore their session just by typing their email address. EventEase looks up their most recent registration and picks up where they left off.
- **Graceful errors** — Navigate to a URL that doesn't exist and you'll land on a proper 404 page instead of a blank screen, with quick links to get back on track.
- **Landing page** — The home page gives new visitors a quick overview of what EventEase is and a peek at three upcoming events to draw them in.

## How it's built

### Pages and components

| Component | URL | What it does |
| --- | --- | --- |
| `Home.razor` | `/` | Landing page — hero section and event preview |
| `Events.razor` | `/events` | The main event listing, paginated |
| `EventDetail.razor` | `/events/{id:int}` | Full details for a single event |
| `EventRegistration.razor` | `/events/{id:int}/register` | Registration form with session pre-fill |
| `MyRegistrations.razor` | `/my-registrations` | Personal attendance tracker |
| `SignIn.razor` | `/sign-in` | Restore session by email for returning users |
| `NotFound.razor` | anything unknown | 404 page |

### Services

Three services hold the app's state for the lifetime of the browser session:

- **`EventService`** — owns the list of 9 sample events; registered as a singleton
- **`RegistrationService`** — stores registrations and handles capacity checks, duplicate detection, registration counts, and email lookups; registered as a singleton
- **`SessionService`** — tracks the active user (name, email, phone), persists the session to `localStorage` via `IJSRuntime` so a page refresh doesn't sign you out, and fires an `OnSessionChanged` event so any subscribed component can re-render immediately; registered as scoped (required because `IJSRuntime` is scoped — in WebAssembly, scoped and singleton are effectively equivalent)

Because `EventService` and `RegistrationService` are singletons, navigating between pages doesn't reset anything — register for an event on the form page, go back to the listing, and the availability bar will already reflect your registration.

### Session state and localStorage persistence

When a user successfully registers for an event, `SessionService.StartSession()` is called with their details. From that point on:

- The nav sidebar shows their name, a **My Registrations** link, and a **Sign Out** button instead of the generic Sign In link.
- The next registration form they open pre-fills their name, email, and phone from the session.
- The session is serialized to `localStorage` under the key `eventease_session`. If the user refreshes the page, `MainLayout.OnInitializedAsync` calls `SessionService.InitializeAsync()` which reads the stored JSON back and restores the session before the first render — so the experience is seamless.

Signing out calls `localStorage.removeItem` and clears the in-memory session object.

Components that need to react to sign-in/sign-out events (`NavMenu`, `MyRegistrations`) subscribe to `ISessionService.OnSessionChanged` in `OnInitialized` and unsubscribe in `Dispose()`, calling `InvokeAsync(StateHasChanged)` in the handler so the UI updates on the correct thread.

### Two-way data binding

The registration form uses Blazor's `@bind-Value` on every field, so the `Registration` model stays in sync with what the user is typing without any manual event handling. When the form is submitted, `DataAnnotationsValidator` validates that bound model against the data annotation attributes on the model class — if anything fails, the relevant error messages appear inline next to the offending field before `HandleValidSubmit` ever runs.

### Visual design

The app uses a custom design system layered on top of Bootstrap 5:

- **Inter** (Google Fonts) replaces the system sans-serif across the entire app, with negative letter-spacing on headings for a tighter editorial feel.
- **CSS custom properties** (`--ee-primary`, `--ee-radius`, `--ee-transition`, etc.) centralize colour and spacing tokens so the whole look can be adjusted in one place.
- **Cards and buttons** lift slightly on hover (`translateY(-3px)` + deeper shadow) to make the interface feel tactile and interactive.
- **Buttons** carry a soft coloured drop-shadow and a smooth `0.2s ease` transition on background, border, transform, and shadow simultaneously.
- **Form controls** have a matching indigo focus glow instead of Bootstrap's default blue.
- **Active nav links** show a white left-border accent and bold weight; hovering any link nudges the text right slightly for a directional cue.
- **Page transitions** — each page content area fades in with a small upward slide (`0.18s ease`) on every navigation, making the single-page routing feel smooth rather than abrupt.
- **Frosted-glass top bar** — the main layout header uses `backdrop-filter: blur(8px)` so content scrolling behind it blurs rather than hard-cutting.

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
| --- | --- |
| Framework | Blazor WebAssembly (.NET 10) |
| Language | C# |
| Styling | Bootstrap 5 + custom CSS design system |
| Fonts | Inter (Google Fonts) |
| Validation | `System.ComponentModel.DataAnnotations` |
| State | In-memory singleton/scoped services + `localStorage` |
| Serialization | `System.Text.Json` (session persistence) |
| Routing | Blazor Router with typed route constraints |

## Academic context

This is the capstone project for **Module Four: Blazor for Front-End Development** in the [Microsoft Full-Stack Developer Professional Certificate](https://www.coursera.org/professional-certificates/microsoft-full-stack-developer) on Coursera. The module covers Blazor's component model, two-way data binding, routing, and form validation — all of which are demonstrated here. GitHub Copilot was used throughout the development process to explore patterns, generate boilerplate, and speed up iteration.
