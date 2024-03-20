﻿// <auto-generated />
using CityPointOfInterest.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityPointOfInterestApi.Migrations
{
    [DbContext(typeof(CityInfoDbContext))]
    [Migration("20240320090053_DataSeeding")]
    partial class DataSeeding
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("CityPointOfInterest.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The one with that big park.",
                            Name = "New York City"
                        },
                        new
                        {
                            Id = 2,
                            Description = "The one with the cathedral that was never really finished.",
                            Name = "Antwerp"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The one with that big tower.",
                            Name = "Paris"
                        });
                });

            modelBuilder.Entity("CityPointOfInterest.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "The most visited urban park in the United States.",
                            Name = "Central Park"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 3,
                            Description = "A wrold-class tower on the Champ de Mars.",
                            Name = "Eiffel Tower"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 3,
                            Description = "The world's largest museum.",
                            Name = "The Louvre"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 3,
                            Description = "A modern museum.",
                            Name = "Museum of Modern Art"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 2,
                            Description = "The most visited urban park in the United States.",
                            Name = "Central Park"
                        },
                        new
                        {
                            Id = 6,
                            CityId = 2,
                            Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.",
                            Name = "Cathedral"
                        },
                        new
                        {
                            Id = 7,
                            CityId = 1,
                            Description = "A 102-story skyscraper located in Midtown Manhattan.",
                            Name = "Empire State Building"
                        });
                });

            modelBuilder.Entity("CityPointOfInterest.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CityPointOfInterest.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CityPointOfInterest.Entities.City", b =>
                {
                    b.Navigation("PointsOfInterest");
                });
#pragma warning restore 612, 618
        }
    }
}
