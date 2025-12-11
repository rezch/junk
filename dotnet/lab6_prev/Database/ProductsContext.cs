using Microsoft.EntityFrameworkCore;
using MvcReprApp.Models;

namespace MvcReprApp.Database
{
    public class ProductsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=products.db");
        }
    }
}