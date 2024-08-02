using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.Infrasctructure.Persistence
{
    public class PlaceRentalDbContext : DbContext
    {
        public PlaceRentalDbContext(DbContextOptions<PlaceRentalDbContext> options) : base(options)
        {
            
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<PlaceAmenity> PlaceAmenities { get; set; }
        public DbSet<PlaceBook> PlaceBooks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Place>(p =>
            {
                p.HasKey(p => p.Id);
                p.HasMany(u => u.Amenities)
                 .WithOne() //tem apenas um place associado
                 .HasForeignKey(b => b.IdPlace)
                 .OnDelete(DeleteBehavior.Restrict);

                p.HasMany(u => u.Books)
                 .WithOne(b => b.Place) //tem apenas um place associado
                 .HasForeignKey(b => b.IdPlace)
                 .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(u => u.User)
                 .WithMany(u => u.Places) //tem apenas um place associado
                 .HasForeignKey(b => b.CreatedBy)
                 .OnDelete(DeleteBehavior.Restrict);

                p.OwnsOne(p => p.Address, a =>
                {
                    a.Property(a => a.Street).HasColumnName("Street");
                    a.Property(a => a.Number).HasColumnName("Number");
                    a.Property(a => a.ZipCode).HasColumnName("ZipCode");
                    a.Property(a => a.District).HasColumnName("District");
                    a.Property(a => a.City).HasColumnName("City");
                    a.Property(a => a.State).HasColumnName("State");
                    a.Property(a => a.Country).HasColumnName("Country");
                });
            });

            builder.Entity<PlaceAmenity>(p =>
            {
                p.HasKey(p => p.Id);
            });

            builder.Entity<PlaceBook>(p =>
            {
                p.HasKey(p => p.Id);
            });

            builder.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);
                u.HasMany(u => u.Books)
                 .WithOne(b => b.User)
                 .HasForeignKey(b => b.IdUser)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }
    }
}
