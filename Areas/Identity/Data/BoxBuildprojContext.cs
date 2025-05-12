using BoxBuildproj.Areas.Identity.Data;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BoxBuildproj.Data;

public class BoxBuildprojContext : IdentityDbContext<BoxBuildprojUser>
{
    public BoxBuildprojContext(DbContextOptions<BoxBuildprojContext> options)
        : base(options)
    {

    }
   //public DbSet<BoxBuildprojUser> Users { get; set; }

    //public DbSet<Product> Products { get; set; }
    //public DbSet<Category> Categories { get; set; }

    //public DbSet<Categoryy> Categoryy { get; set; }

    //public DbSet<Categ> Categ { get; set; } // Add Category Table

    //public DbSet<Student> Students { get; set; } // Add Student table

    //public DbSet<Producttbl> Producttbl { get; set; }


    // public DbSet<catgy> Catgy { get; set; }

    public DbSet<Productstbl> Productstbl { get; set; }

    public DbSet<Categorytbl> Categorytbl { get; set; }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }

    public DbSet<Orders> Orders { get; set; }
    public DbSet<Payments> Payments { get; set; }

    //public DbSet<Product> Products { get; set; }

    //public DbSet<Prodd> Prodd { get; set; }  // New Prodd tabl

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);
    //        // Customize the ASP.NET Identity model and override the defaults if needed.
    //        // For example, you can rename the ASP.NET Identity table names and more.
    //        // Add your customizations after calling base.OnModelCreating(builder);
    //        // Configure relationships
    //        // Ensure Identity tables use existing names
    //        //builder.Entity<IdentityUser>().ToTable("AspNetUsers");
    //        //builder.Entity<IdentityRole>().ToTable("AspNetRoles");
    //        //builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
    //        //builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
    //        //builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
    //        //builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
    //        //builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");

    //        builder.Entity<BoxBuildprojUser>()
    //        .ToTable("AspNetUsers");  // Map BoxBuildprojUser to the Identity table

    //        builder.Entity<IdentityUser>()
    //            .ToTable("AspNetUsers");  // Ensure IdentityUser uses the same table
    //        // Configure the Product table
    //        builder.Entity<Product>()
    //            .Property(p => p.Price)
    //            .HasColumnType("decimal(10,2)");  // Fix precision for Price

    //        // Foreign key with Identity table
    //        builder.Entity<Product>()
    //            .HasOne(p => p.Category)
    //            .WithMany(c => c.Products)
    //            .HasForeignKey(p => p.CategoryId);
    //    }
    //}
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //// Only map BoxBuildprojUser to AspNetUsers (DO NOT map IdentityUser separately)
    //builder.Entity<BoxBuildprojUser>()
    //    .ToTable("AspNetUsers");

    //// Remove this line to prevent conflict:
    //// builder.Entity<IdentityUser>().ToTable("AspNetUsers");

    //// Configure the Product table
    //builder.Entity<Product>()
    //    .Property(p => p.Price)
    //    .HasColumnType("decimal(10,2)");  // Fix precision for Price

    //// Foreign key with Identity table
    //builder.Entity<Product>()
    //    .HasOne(p => p.Category)
    //    .WithMany(c => c.Products)
    //.HasForeignKey(p => p.CategoryId);

    //// Define foreign key relationship
    //builder.Entity<Product>()
    //    .HasOne(p => p.Catgy)
    //    .WithMany(c => c.Products)
    //    .HasForeignKey(p => p.CategoryId)
    //    .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  // Ensure this is called to configure Identity correctly

        // Any additional custom configurations for other entities can go here
    }
}
