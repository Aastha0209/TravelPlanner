using Microsoft.EntityFrameworkCore;
using TravelPlanner.Models;

namespace TravelPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Itinerary> Itineraries { get; set; } = null!;
        public DbSet<TripImage> TripImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User: Email should be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // User -> Trips
            modelBuilder.Entity<User>()
                .HasMany(u => u.Trips)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trip -> Itineraries
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Itineraries)
                .WithOne(i => i.Trip)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: default table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Trip>().ToTable("Trips");
            modelBuilder.Entity<Itinerary>().ToTable("Itineraries");
        }
    }
}
