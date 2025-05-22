using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_Restaurant.Data;

namespace Tema3_Restaurant.Helpers
{
    public static class DbContextFactory
    {
        private static readonly string _connectionString =
            @"Server=localhost;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static RestaurantContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new RestaurantContext(options);
        }
    }
}
