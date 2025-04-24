using Kuzmich.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuzmich.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<Laptop> Laptops { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
