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
//        protected override void OnConfiguring(DbContextOptionsBuilder
//optionsBuilder)
//        {
//            base.OnConfiguring(optionsBuilder);
//            optionsBuilder.UseSqlServer("");
//        }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}