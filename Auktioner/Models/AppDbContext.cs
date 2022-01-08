using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Auktioner.Models
{
    public class AppDbContext : IdentityDbContext<Customer>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }


        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SellerBuyer> SellerBuyers { get; set; }
        public DbSet <SellingBuyingHistory> sellingBuyingHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, CategoryName = "Vehicle", Description = "Some Description To User" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 2, CategoryName = "Home", Description = "Some Description To User" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 3, CategoryName = "Decoration", Description = "Some Description To User" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 4, CategoryName = "Tools", Description = "Some Description To User" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 5, CategoryName = "Others", Description = "Some Description To User" });

            modelBuilder.Entity<Inventory>().HasData(new Inventory
            {
                InventoryId = 1,
                SpecialId = "LBN 398212",
                InventoryName = "Chevrolet Chevelle",
                InventoryImage = "https://dealeraccelerate-all.s3.amazonaws.com/ccl/images/2/7/7/7/2777/7094441e5c98c_hd_1965-chevrolet-chevelle.jpg",
                InventoryDecade = 1965,
                StartPrice = 1000,
                Description = "Some Description",
                Status = "Auction started",
                CategoryId = 1,
                CustomerId = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
            });

            modelBuilder.Entity<Inventory>().HasData(new Inventory
            {
                InventoryId = 2,
                SpecialId = "XKL 391212",
                InventoryName = "Pair of Spanish Rattan Wicker",
                InventoryImage = "https://a.1stdibscdn.com/1950-pair-of-spanish-rattan-wicker-rocking-chairs-for-sale-picture-2/f_30443/1548251429782/IMG_1431_master.JPG?width=768",
                InventoryDecade = 1950,
                StartPrice = 1200,
                Description = "Some Description",
                Status = "Auction started",
                CategoryId = 2,
                CustomerId = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
            });

            modelBuilder.Entity<Inventory>().HasData(new Inventory
            {
                InventoryId = 3,
                SpecialId = "RHS 350002",
                InventoryName = "VINTAGE 1960'S CHRISTMAS",
                InventoryImage = "https://th.bing.com/th/id/R.1fb4732bb13413e836888362f234b0d6?rik=HSAfh3Fk9yUYQQ&riu=http%3a%2f%2fi.ebayimg.com%2fimages%2fi%2f230982009514-0-1%2fs-l1000.jpg&ehk=Gxy9K%2bBYj4kgTcLesRqXrApAjCLHB%2bOPlL80mxcoFxo%3d&risl=&pid=ImgRaw&r=0",
                InventoryDecade = 1960,
                StartPrice = 500,
                Description = "Some Description",
                Status = "Auction started",
                CategoryId = 3,
                CustomerId = "a25be9b0-aa65-4af8-bd17-00bd9344e575"
            });

            modelBuilder.Entity<Inventory>().HasData(new Inventory
            {
                InventoryId = 4,
                SpecialId = "LPM 398019",
                InventoryName = "Mercedes Tool kit",
                InventoryImage = "https://assets.catawiki.nl/assets/2018/11/28/4/d/e/4deee29c-298b-414e-900f-0809cc12a192.jpg",
                InventoryDecade = 1980,
                StartPrice = 900,
                Description = "Some Description",
                Status = "Auction started",
                CategoryId = 4,
                CustomerId = "a25be9b0-aa65-4af8-bd17-00bd9344e575"
            });

            modelBuilder.Entity<Inventory>().HasData(new Inventory
            {
                InventoryId = 5,
                SpecialId = "MVN 182212",
                InventoryName = "Cassette Player",
                InventoryImage = "https://dealeraccelerate-all.s3.amazonaws.com/ccl/images/2/7/7/7/2777/7094441e5c98c_hd_1965-chevrolet-chevelle.jpg",
                InventoryDecade = 1982,
                StartPrice = 300,
                Description = "Some Description",
                Status = "Auction started",
                CategoryId = 5,
                CustomerId = "a25be9b0-aa65-4af8-bd17-00bd9344e575"
            });

            //Insert New Role
            const string AdminId = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string RoleId = AdminId;
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = RoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            //Add the First User
            var hasher = new PasswordHasher<Customer>();
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = AdminId,
                UserName = "Admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "Admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                City = "Linköping",
                Country = "Sweden",
                Address="Prästbolsgatan 21",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                SecurityStamp = string.Empty
            });

            // Make the first user as an admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = RoleId,
                UserId = AdminId
            });

            // Add second user (Just user not an admin)
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = "a25be9b0-aa65-4af8-bd17-00bd9344e575",
                UserName = "user@gmail.com",
                NormalizedUserName = "USER@GMAIL.COM",
                Email = "USER@gmail.com",
                FirstName = "User",
                LastName = "User",
                City = "Linköping",
                Country = "Sweden",
                Address = "Prästbolsgatan 21",
                NormalizedEmail = "USER@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "User123!"),
                SecurityStamp = string.Empty
            });






        }

    }
}
