using Microsoft.EntityFrameworkCore;
using TravelPlanner.Data;
using TravelPlanner.Models;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// ✅ Use SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// ✅ Auto-create & seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();

    // ✅ Step 1: Create a default user if none exists
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Name = "Demo User",
            Email = "demo@travelplanner.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("demo123"), // ✅ Properly hashed
            ProfilePicture = null
        });
        db.SaveChanges();
    }

    // ✅ Step 2: Seed trips only if none exist
    if (!db.Trips.Any())
    {
        var defaultUser = db.Users.First(); // get demo user

        var sampleTrips = new List<Trip>
        {
            new Trip
            {
                Name = "Paris Getaway",
                Destination = "Paris, France",
                StartDate = new DateTime(2025, 11, 10),
                EndDate = new DateTime(2025, 11, 17),
                UserId = defaultUser.UserId,
                Itineraries = new List<Itinerary>
                {
                    new Itinerary { Type = "Place", Name = "Eiffel Tower", Notes = "Evening visit for sunset view", Date = new DateTime(2025, 11, 11) },
                    new Itinerary { Type = "Restaurant", Name = "Le Jules Verne", Notes = "Dinner with city view", Date = new DateTime(2025, 11, 11) },
                    new Itinerary { Type = "Activity", Name = "Seine River Cruise", Notes = "Night cruise", Date = new DateTime(2025, 11, 12) }
                }
            },
            new Trip
            {
                Name = "Tokyo Adventure",
                Destination = "Tokyo, Japan",
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 10),
                UserId = defaultUser.UserId,
                Itineraries = new List<Itinerary>
                {
                    new Itinerary { Type = "Place", Name = "Shibuya Crossing", Notes = "Explore nearby cafes", Date = new DateTime(2025, 12, 2) },
                    new Itinerary { Type = "Activity", Name = "Mount Fuji Day Trip", Notes = "Book bus in advance", Date = new DateTime(2025, 12, 4) }
                }
            },
            new Trip
            {
                Name = "Bali Escape",
                Destination = "Bali, Indonesia",
                StartDate = new DateTime(2026, 1, 5),
                EndDate = new DateTime(2026, 1, 15),
                UserId = defaultUser.UserId,
                Itineraries = new List<Itinerary>
                {
                    new Itinerary { Type = "Place", Name = "Ubud Monkey Forest", Notes = "Arrive early to avoid crowds", Date = new DateTime(2026, 1, 6) },
                    new Itinerary { Type = "Activity", Name = "Snorkeling at Nusa Penida", Notes = "Pack underwater camera", Date = new DateTime(2026, 1, 8) },
                    new Itinerary { Type = "Restaurant", Name = "Bambu Indah", Notes = "Dinner with rice field view", Date = new DateTime(2026, 1, 9) }
                }
            }
        };

        db.Trips.AddRange(sampleTrips);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
