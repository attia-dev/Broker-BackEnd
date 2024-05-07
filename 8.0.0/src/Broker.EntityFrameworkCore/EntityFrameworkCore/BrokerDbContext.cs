using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Broker.Authorization.Roles;
using Broker.Authorization.Users;
using Broker.MultiTenancy;
using Broker.Lookups;
using Broker.Customers;
using Broker.Payments;
using Broker.Advertisements;
using Broker.RateUs;
using Broker.SocialContacts;
using Broker.Notifications;

namespace Broker.EntityFrameworkCore
{
    public class BrokerDbContext : AbpZeroDbContext<Tenant, Role, User, BrokerDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<BrokerPerson> BrokerPersons { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Seeker> Seekers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Duration> Durations { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

       // public DbSet<Image> Images { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
       // public DbSet<AdvertisementDecoration> AdvertisementDecorations { get; set; }
       //  public DbSet<AdvertisementDocument> AdvertisementDocuments { get; set; }
        public DbSet<AdvertisementFacility> AdvertisementFacilities { get; set; }
        //public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<AdView> AdViews { get; set; }
        public DbSet<AdFavorite> AdFavorites { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Broker.ContactUs.ContactUs> ContactUses { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<DurationBuildingType> DurationBuildingTypes { get; set; }
        public DbSet<SocialContact> SocialContacts { get; set; }
        public DbSet<AdvertisementBooking> AdvertisementBookings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPhoto> ProjectPhotos { get; set; }
        public DbSet<ProjectLayout> ProjectLayouts { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public BrokerDbContext(DbContextOptions<BrokerDbContext> options)
            : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Advertisement>()
        //        .HasMany(a => a.views)
        //        .WithOne()
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Advertisement>()
        //        .HasMany(a => a.Photos)
        //        .WithOne()
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Advertisement>()
        //        .HasMany(a => a.Layouts)
        //        .WithOne()
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Advertisement>()
        //        .HasMany(a => a.AdvertisementFacilites)
        //        .WithOne()
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Advertisement>()
        //        .HasMany(a => a.AdvertisementBookingsList)
        //        .WithOne()
        //        .OnDelete(DeleteBehavior.Cascade);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
