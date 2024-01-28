using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DyersCargoTransit_API.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       public DbSet<UserProfile>UserProfiles { get; set; }
        
        //cargo
        public DbSet<Cargo> Cargos { get; set; } //
        public DbSet<CargoStatus> CargoStatuses { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }

        //customer shipping  
        public DbSet<Customer> Customers { get; set; } //
        public DbSet<Customer_Shipment> CustomerShipments { get; set; } //
        public DbSet<Customer_ShipmentStatus> CustomerShipmentStatuses { get; set;}
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<ShippingSchedule> ShippingSchedules { get; set; } //

        //truck
        public DbSet<Truck> Trucks { get; set; } //
        public DbSet<TruckRoute> TruckRoutes { get; set; } //
        public DbSet<TruckStatus> TruckStatuses { get; set; }


        //Below, I use the HasKey method to specify that UserProfile.
        //ApplicationUserId is the key for the UserProfile entity.
        //Then, we use the HasOne and WithOne methods to configure the one-to-one relationship,
        //specifying the foreign key property (UserProfile.ApplicationUserId) and the navigation property (UserProfile.User).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasKey(u => u.ApplicationUserId);

            modelBuilder.Entity<UserProfile>()
                .HasOne(u => u.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(u => u.ApplicationUserId);
        }
    }
}
