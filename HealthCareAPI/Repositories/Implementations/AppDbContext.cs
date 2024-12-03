using HealthCareABApi.Models;
using HealthCareABApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace HealthCareABApi.Repositories.Implementations
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseSerialColumns();

            // Converter for roles in the User entity
            var rolesConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<string>());

            modelBuilder.Entity<User>()
                .Property(e => e.Roles)
                .HasConversion(rolesConverter)
                .HasColumnType("jsonb");

            // Converter for available slots in the Availability entity
            var availableSlotConverter = new ValueConverter<List<AvailableSlot>, string>(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                v => JsonSerializer.Deserialize<List<AvailableSlot>>(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) ?? new List<AvailableSlot>());

            modelBuilder.Entity<Availability>()
                .Property(a => a.AvailableSlots)
                .HasConversion(availableSlotConverter)
                .HasColumnType("jsonb");

            // Converter for DateTime in the Appointment entity
            var dateTimeConverter = new ValueConverter<DateTime, string>(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                v => JsonSerializer.Deserialize<DateTime>(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );

            modelBuilder.Entity<Appointment>()
                .Property(a => a.DateTime)
                .HasConversion(dateTimeConverter)
                .HasColumnType("jsonb");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
