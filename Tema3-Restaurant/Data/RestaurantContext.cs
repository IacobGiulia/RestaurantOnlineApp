using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tema3_Restaurant.Models;
using BCrypt.Net;
using System.Reflection;
using System.Windows.Controls;

namespace Tema3_Restaurant.Data
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Tema3_Restaurant.Models.Menu> Menus { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductAllergen> ProductAllergens { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ConfigurationApp> ConfigurationApp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductAllergen>().HasKey(pa => new { pa.ProductID, pa.AllergenID });
            modelBuilder.Entity<MenuProduct>().HasKey(mp => new { mp.MenuID, mp.ProductID });
            modelBuilder.Entity<ProductAllergen>().HasOne(pa => pa.Product).WithMany(p => p.ProductAllergens).HasForeignKey(pa => pa.ProductID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductAllergen>().HasOne(pa => pa.Allergen).WithMany(a => a.ProductAllergens).HasForeignKey(pa => pa.AllergenID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MenuProduct>().HasOne(mp => mp.Menu).WithMany(p => p.MenuProducts).HasForeignKey(mp => mp.MenuID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MenuProduct>().HasOne(mp => mp.Product).WithMany(p => p.MenuProducts).HasForeignKey(mp => mp.ProductID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductImage>().HasOne(ip => ip.Product).WithMany(p => p.Images).HasForeignKey(ip => ip.ProductID);
            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserID);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderID);

            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ConfigurationApp>().HasData(
                new ConfigurationApp { ID = 1, Key = "ProcentReducereMeniu", Value = "10" },
                new ConfigurationApp { ID = 2, Key = "ValoareMinimaTransportGratuit", Value = "100" },
                new ConfigurationApp { ID = 3, Key = "CostTransport", Value = "15" },
                new ConfigurationApp { ID = 4, Key = "NumarComenziPentruDiscount", Value = "5" },
                new ConfigurationApp { ID = 5, Key = "IntervalZilePentruDiscount", Value = "30" },
                new ConfigurationApp { ID = 6, Key = "ProcentDiscountComenziMultiple", Value = "5" },
                new ConfigurationApp { ID = 7, Key = "CantitateMinimaPreparat", Value = "5" },
                new ConfigurationApp { ID = 8, Key = "ProcentReducereComanda", Value = "10"},
                new ConfigurationApp { ID = 9, Key = "ValoareMinimaPentruReducere", Value = "150"},
                new ConfigurationApp { ID = 10,Key = "PragMinimStoc", Value = "1000"}
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Mic dejun" },
                new Category { ID = 2, Name = "Supe/Ciorbe" },
                new Category { ID = 3, Name = "Fel principal" },
                new Category { ID = 4, Name = "Desert" },
                new Category { ID = 5, Name = "Bauturi" }
            );

            // Add seed data for alergeni
            modelBuilder.Entity<Allergen>().HasData(
                new Allergen { ID = 1, Name = "Gluten" },
                new Allergen { ID = 2, Name = "Lactoza" },
                new Allergen { ID = 3, Name = "Oua" },
                new Allergen { ID = 4, Name = "Telina" },
                new Allergen { ID = 5, Name = "Peste" },
                new Allergen { ID = 6, Name = "Nuci" },
                new Allergen { ID = 7, Name = "Soia"}

            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ID = 1, Name = "Omleta", CategoryID = 1, Price = 20.00m, PortionQuantity = 250, TotalQuantity = 3500, Available = true},
                new Product { ID = 2, Name = "Clatite", CategoryID = 1, Price = 30.00m, PortionQuantity = 300, TotalQuantity = 6000, Available = true},
                new Product { ID = 3, Name = "Cascaval Pane", CategoryID = 1, Price = 15.50m, PortionQuantity = 150, TotalQuantity = 7500, Available = true},
                new Product { ID = 4, Name = "Ciorba de perisoare", CategoryID = 2, Price = 10.00m, PortionQuantity = 300, TotalQuantity = 3000, Available = true},
                new Product { ID = 5, Name = "Supa de galuste", CategoryID = 2, Price = 20.50m, PortionQuantity = 300, TotalQuantity = 3000, Available = true},
                new Product { ID = 6, Name = "Ciorba de burta", CategoryID = 2, Price = 15.50m, PortionQuantity = 300, TotalQuantity = 3000, Available = true},
                new Product { ID = 7, Name = "Piept de pui la gratar", CategoryID = 3, Price = 15.00m, PortionQuantity = 200, TotalQuantity = 1000, Available = true},
                new Product { ID = 8, Name = "Pulpe de pui", CategoryID = 3, Price = 15.50m, PortionQuantity = 200, TotalQuantity = 2000, Available = true},
                new Product { ID = 9, Name = "Burger de vita", CategoryID = 3, Price = 30.00m, PortionQuantity = 500, TotalQuantity = 5000, Available = true},
                new Product { ID = 10, Name = "Pastrav afumat", CategoryID = 3, Price = 40.00m, PortionQuantity = 300, TotalQuantity = 4000, Available = true},
                new Product { ID = 11, Name = "Cartofi prajiti", CategoryID = 3, Price = 10.00m, PortionQuantity = 200, TotalQuantity = 8000, Available = true},
                new Product { ID = 12, Name = "Sarmale", CategoryID = 3, Price = 35.00m, PortionQuantity = 350, TotalQuantity = 6000, Available = true},
                new Product { ID = 13, Name = "Mamaliga", CategoryID = 3, Price = 15.50m, PortionQuantity = 200, TotalQuantity = 4000, Available = true},
                new Product { ID = 14, Name = "Inghetata", CategoryID = 4, Price = 20.00m, PortionQuantity = 150, TotalQuantity = 3000, Available = true},
                new Product { ID = 15, Name = "Papanasi", CategoryID = 4, Price = 35.00m, PortionQuantity = 250, TotalQuantity = 5000, Available = true},
                new Product { ID = 16, Name = "Tarta cu fructe", CategoryID = 4, Price = 20.00m, PortionQuantity = 200, TotalQuantity = 4000, Available = true},
                new Product { ID = 17, Name = "Fanta", CategoryID = 5, Price = 5.00m, PortionQuantity = 100, TotalQuantity = 3000, Available = true},
                new Product { ID = 18, Name = "Pepsi", CategoryID = 5, Price = 5.00m, PortionQuantity = 100, TotalQuantity = 3000, Available = true},
                new Product { ID = 19, Name = "Apa plata", CategoryID = 5, Price = 3.00m, PortionQuantity = 100, TotalQuantity = 3000, Available = true },
                new Product { ID = 20, Name = "Espresso", CategoryID = 5, Price = 10.00m, PortionQuantity = 100, TotalQuantity = 3000, Available = true}

                


                );

            modelBuilder.Entity<ProductImage>().HasData(

                new ProductImage { ID = 1, ProductID = 1, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\omleta.jpg" },
                new ProductImage { ID = 2, ProductID = 2, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\clatita.jpg" },
                new ProductImage { ID = 3, ProductID = 3, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\cascavalpane.jpg" },
                new ProductImage { ID = 4, ProductID = 4, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\ciorbaperisoare.jpg" },
                new ProductImage { ID = 5, ProductID = 5, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\supagaluste.jpg" },
                new ProductImage { ID = 6, ProductID = 6, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\ciorbaburta.jpg" },
                new ProductImage { ID = 7, ProductID = 7, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pieptpui.jpg" },
                new ProductImage { ID = 8, ProductID = 8, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pulpepui.jpg" },
                new ProductImage { ID = 9, ProductID = 9, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\burger.jpg" },
                new ProductImage { ID = 10, ProductID = 10, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pastrav.jpg" },
                new ProductImage { ID = 11, ProductID = 11, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\cartofiprajiti.jpg" },
                new ProductImage { ID = 12, ProductID = 12, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\sarmale.jpg" },
                new ProductImage { ID = 13, ProductID = 13, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\mamaliga.jpg" },
                new ProductImage { ID = 14, ProductID = 14, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\inghetata.jpg" },
                new ProductImage { ID = 15, ProductID = 15, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\papanasi.jpg" },
                new ProductImage { ID = 16, ProductID = 16, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\tarta.jpg" },
                new ProductImage { ID = 17, ProductID = 17, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\fanta.jpg" },
                new ProductImage { ID = 18, ProductID = 18, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pepsi.jpg" },
                new ProductImage { ID = 19, ProductID = 19, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\apa.jpg" },
                new ProductImage { ID = 20, ProductID = 20, Path = "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\espresso.jpg" }
                
                
                );



            // Add seed data for utilizatori
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = 1,
                    FirstName = "Alexandru",
                    LastName = "Ionescu",
                    Email = "alexandruionescu@restaurant.com",
                    Phone = "07630019099",
                    Address = "Adresa restaurant",
                    Password = "admin123",
                    Role = "Angajat"
                },
                new User
                {
                    ID = 2,
                    FirstName = "Maria",
                    LastName = "Popescu",
                    Email = "mariapopescu@email.com",
                    Phone = "07123456789",
                    Address = "Str. Eroilor 30A",
                    Password = "maria123",
                    Role = "Client"
                },

                new User
                {
                    ID = 3,
                    FirstName = "Giulia",
                    LastName = "Iacob",
                    Email = "giuliaiacob@email.com",
                    Phone = "0768300292",
                    Address = "Str. Garii 28B",
                    Password = "giulia123",
                    Role = "Client"

                },

                new User
                {
                    ID = 4,
                    FirstName = "Vlad",
                    LastName = "Achim",
                    Email = "vladachim@restaurant.com",
                    Phone = "07856321000",
                    Address = "Adresa restaurant",
                    Password = "admin123",
                    Role = "Angajat"
                }
            );

            modelBuilder.Entity<ProductAllergen>().HasData(
                new ProductAllergen { ProductID = 1, AllergenID = 3 },
                new ProductAllergen { ProductID = 2, AllergenID = 1},
                new ProductAllergen { ProductID = 2, AllergenID = 2},
                new ProductAllergen { ProductID = 2, AllergenID = 3},
                new ProductAllergen { ProductID = 3, AllergenID = 2 },
                new ProductAllergen { ProductID = 3, AllergenID = 3},
                new ProductAllergen { ProductID = 4, AllergenID = 4},
                new ProductAllergen { ProductID = 5, AllergenID = 1},
                new ProductAllergen { ProductID = 6, AllergenID = 2},
                new ProductAllergen { ProductID = 9, AllergenID = 1},
                new ProductAllergen { ProductID = 10, AllergenID = 5},
                new ProductAllergen { ProductID = 14, AllergenID = 2},
                new ProductAllergen { ProductID = 15, AllergenID = 1},
                new ProductAllergen { ProductID = 15, AllergenID = 2 },
                new ProductAllergen { ProductID = 15, AllergenID = 3 },
                new ProductAllergen { ProductID = 16, AllergenID = 1},
                new ProductAllergen { ProductID = 16, AllergenID = 2}


                );

            modelBuilder.Entity<Tema3_Restaurant.Models.Menu>().HasData(
                new Models.Menu { ID = 1, Name = "Meniu Mic Dejun", CategoryID = 1, Available = true },
                new Models.Menu { ID = 2, Name = "Meniu Piept de Pui", CategoryID = 3, Available = true},
                new Models.Menu { ID = 3, Name = "Meniu Pulpe de Pui", CategoryID = 3, Available = true},
                new Models.Menu { ID = 4, Name = "Meniu Traditional", CategoryID = 3, Available = true},
                new Models.Menu { ID = 5, Name = "Meniu Pastrav", CategoryID = 3, Available = true},
                new Models.Menu { ID = 6, Name = "Meniu Burger", CategoryID = 3, Available = true}
                );

            modelBuilder.Entity<MenuProduct>().HasData(
                new MenuProduct { MenuID = 1, ProductID = 1, Quantity = 150},
                new MenuProduct { MenuID = 1, ProductID = 2, Quantity = 150},
                new MenuProduct { MenuID = 1, ProductID = 20, Quantity = 100},

                new MenuProduct { MenuID = 2, ProductID = 7, Quantity = 200},
                new MenuProduct { MenuID = 2, ProductID = 11, Quantity = 200},
                new MenuProduct { MenuID = 2, ProductID = 17, Quantity = 100},

                new MenuProduct { MenuID = 3, ProductID = 8, Quantity = 200},
                new MenuProduct { MenuID = 3, ProductID = 11, Quantity = 200},
                new MenuProduct { MenuID = 3, ProductID = 18, Quantity = 100},

                new MenuProduct { MenuID = 4, ProductID = 4, Quantity = 100},
                new MenuProduct { MenuID = 4, ProductID = 12, Quantity = 150},
                new MenuProduct { MenuID = 4, ProductID = 13, Quantity = 150},
                new MenuProduct { MenuID = 4, ProductID = 19, Quantity = 100},

                new MenuProduct { MenuID = 5, ProductID = 10, Quantity = 200},
                new MenuProduct { MenuID = 5, ProductID = 11, Quantity = 200},
                new MenuProduct { MenuID = 5, ProductID = 17, Quantity = 100},

                new MenuProduct { MenuID = 6, ProductID = 9, Quantity = 350},
                new MenuProduct { MenuID = 6, ProductID = 11, Quantity = 200},
                new MenuProduct { MenuID = 6, ProductID = 18, Quantity = 100}





                );
        }
    }
}
