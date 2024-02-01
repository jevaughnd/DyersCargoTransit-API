using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DyersCargoTransit_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       

        //cargo
        public DbSet<Cargo> Cargos { get; set; } //
        public DbSet<CargoStatus> CargoStatuses { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }

        //customer shipping  
        public DbSet<Customer> Customers { get; set; } //
        public DbSet<Customer_Shipment> CustomerShipments { get; set; } //
        public DbSet<Customer_ShipmentStatus> CustomerShipmentStatuses { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<ShippingSchedule> ShippingSchedules { get; set; } //

        //truck
        public DbSet<Truck> Trucks { get; set; } //
        public DbSet<TruckRoute> TruckRoutes { get; set; } //
        public DbSet<TruckStatus> TruckStatuses { get; set; }

        public DbSet<CustomerProfile> CustomerProfiles { get; set; }

    }


        
}
