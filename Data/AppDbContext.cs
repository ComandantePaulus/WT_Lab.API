using Microsoft.EntityFrameworkCore;
using WT_Lab.API.Data;
using WT_Lab.Domain;
namespace WT_Lab.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Asset> Asset { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
    }
}