using CityPointOfInterest.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityPointOfInterest.DataContext
{
    public class CityInfoDbContext : DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("connectionString");
        //     base.OnConfiguring(optionsBuilder);
        // }
    }
}