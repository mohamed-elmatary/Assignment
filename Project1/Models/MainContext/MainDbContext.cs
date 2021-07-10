
using Microsoft.EntityFrameworkCore;
using Project1.Models.Models;

namespace Project1.Models.MainContext
{
    public class MainDbContext : DbContext
    {
        public static string connectionString;

        public MainDbContext()
        {

        }
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Step> Steps { get; set; }


    }
}
