using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Billing> Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vehicle>()
                .Property(v => v.PricePerDay)
                .HasPrecision(18, 2);

            builder.Entity<Billing>()
                .Property(b => b.BaseAmount)
                .HasPrecision(18, 2);

            builder.Entity<Billing>()
                .Property(b => b.Tax)
                .HasPrecision(18, 2);

            builder.Entity<Billing>()
                .Property(b => b.AdditionalCharges)
                .HasPrecision(18, 2);

            builder.Entity<Billing>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);
        }
    }
}