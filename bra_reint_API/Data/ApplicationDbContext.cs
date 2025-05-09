using bra_reint_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bra_reint_API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingType> BookingTypes { get; set; }
    public DbSet<BookingToBookingType> BookingBookingTypes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PostalCode> PostalCodes { get; set; }
    public DbSet<SpecialAvailabilityDate> SpecialAvailabilityDates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        /*
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        if (string.IsNullOrEmpty(adminPassword))
        {
            throw new InvalidOperationException("Admin password is not configured.");
        }
        
        // SEEDING PREPARATION
        var hasher = new PasswordHasher<IdentityUser>();
        const string userName = "chonvitt@hotmail.no";
        
        var adminUser = new IdentityUser
        {
            UserName = userName,
            NormalizedUserName = userName.ToUpper(),
            Email = userName,
            NormalizedEmail = userName.ToUpper(),
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, adminPassword),
            SecurityStamp = string.Empty
        };
        
        builder.Entity<IdentityUser>().HasData(adminUser);
        */
        
        
        // SEEDING
        
        builder.Entity<BookingToBookingType>()
            .HasKey(bb => new { bb.BookingId, bb.BookingTypeId });

        builder.Entity<BookingToBookingType>()
            .HasOne(bb => bb.Booking)
            .WithMany(b => b.BookingBookingTypes)
            .HasForeignKey(bb => bb.BookingId);

        builder.Entity<BookingToBookingType>()
            .HasOne(bb => bb.BookingType)
            .WithMany(bt => bt.BookingToBookingTypes)
            .HasForeignKey(bb => bb.BookingTypeId);

        
        // SEEDING POSTAL CODES FOR SANDNESSJØEN
        builder.Entity<PostalCode>().HasData(
            new PostalCode { Code = "8800", City = "Sandnessjøen" },
            new PostalCode { Code = "8801", City = "Sandnessjøen" },
            new PostalCode { Code = "8802", City = "Sandnessjøen" },
            new PostalCode { Code = "8803", City = "Sandnessjøen" },
            new PostalCode { Code = "8804", City = "Sandnessjøen" },
            new PostalCode { Code = "8805", City = "Sandnessjøen" }
        );

        // // SEEDING BOOKING TYPES (Change later and/or add controller and access for admin)
        builder.Entity<BookingType>().HasData(
            new BookingType
            {
                Id = 1, TypeName = "Vasking av vinduer", Description = "Vasker vinduer nøye og bra.", Price = 5000m
            },
            new BookingType
            {
                Id = 2, TypeName = "Spyling tak", Description = "Spyler tak rent for mose og shit.", Price = 7500m
            },
            new BookingType
            {
                Id = 3, TypeName = "Vask søppelbøtter", Description = "Vasker søppelbøtter rene og luktfrie.", Price = 500m
            },
            new BookingType
            {
                Id = 4, TypeName = "Full service", Description = "Alle våre servicer i en bestilling.", Price = 20000m
            },
            new BookingType
            {
                Id = 5, TypeName = "Vask husvegger", Description = "Vasker husvegger. Perfekt før maling.", Price = 7500m
            }
        );
    }
}

