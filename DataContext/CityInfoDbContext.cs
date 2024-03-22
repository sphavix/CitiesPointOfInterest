using CityPointOfInterest.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CityPointOfInterest.DataContext
{
    public class CityInfoDbContext : DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("connectionString");
        //     base.OnConfiguring(optionsBuilder);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(c =>
            {
                c.HasKey(ci => ci.Id);
                c.Property(ci => ci.CityName);
                c.Property(ci => ci.Description);
            });
            modelBuilder.Entity<PointOfInterest>(p =>
            {
                p.HasKey(pi => pi.Id);
                p.Property(pi => pi.PointOfInterestName);
                p.Property(pi => pi.Description);
            });

            modelBuilder.Entity<City>().HasData(
            new City("New York City")
            {
                Id = 1,
                Description = "The one with that big park."
            },
            new City("Antwerp")
            {
                Id = 2,
                Description = "The one with the cathedral that was never really finished."
            },
            new City("Paris")
            {
                Id = 3,
                Description = "The one with that big tower."
            });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("Central Park")
                    {
                        Id = 1,
                        CityId = 1,
                        Description = "The most visited urban park in the United States."
                    },
                    new PointOfInterest("Eiffel Tower")
                    {
                        Id = 2,
                        CityId = 3,
                        Description = "A wrold-class tower on the Champ de Mars."
                    },
                    new PointOfInterest("The Louvre")
                    {
                        Id = 3,
                        CityId = 3,
                        Description = "The world's largest museum."
                    },
                    new PointOfInterest("Museum of Modern Art")
                    {
                        Id = 4,
                        CityId = 3,
                        Description = "A modern museum."
                    },
                    new PointOfInterest("Central Park")
                    {
                        Id = 5,
                        CityId = 2,
                        Description = "The most visited urban park in the United States."
                    },
                    new PointOfInterest("Cathedral")
                    {
                        Id = 6,
                        CityId = 2,
                        Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                    },
                    new PointOfInterest("Empire State Building")
                    {
                        Id = 7,
                        CityId = 1,
                        Description = "A 102-story skyscraper located in Midtown Manhattan."
                    });

            base.OnModelCreating(modelBuilder); // this is needed to call the base method.
        }
    }
}