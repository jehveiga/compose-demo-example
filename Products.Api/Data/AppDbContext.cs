using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api.Data
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
