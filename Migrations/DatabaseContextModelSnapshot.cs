﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TheElectricCityAPI.Models;

namespace TheElectricCityAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TheElectricCityAPI.Models.InventoryItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateOrdered")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ItemDescription")
                        .HasColumnType("text");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("text");

                    b.Property<int>("NumberInStock")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<string>("ProductType")
                        .HasColumnType("text");

                    b.Property<string>("SKU")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AddressCity")
                        .HasColumnType("text");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("text");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("text");

                    b.Property<string>("AddressState")
                        .HasColumnType("text");

                    b.Property<string>("AddressZip")
                        .HasColumnType("text");

                    b.Property<string>("ManagerName")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.LocationItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("InventoryItemID")
                        .HasColumnType("integer");

                    b.Property<int>("LocationID")
                        .HasColumnType("integer");

                    b.Property<int>("NumberInStock")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("InventoryItemID");

                    b.HasIndex("LocationID");

                    b.ToTable("LocationItems");
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("OrderEmail")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("LocationId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.OrderItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("InventoryItemID")
                        .HasColumnType("integer");

                    b.Property<int>("OrderID")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityOrdered")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("InventoryItemID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.LocationItem", b =>
                {
                    b.HasOne("TheElectricCityAPI.Models.InventoryItem", "InventoryItem")
                        .WithMany("LocationItems")
                        .HasForeignKey("InventoryItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheElectricCityAPI.Models.Location", "Location")
                        .WithMany("LocationItems")
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.Order", b =>
                {
                    b.HasOne("TheElectricCityAPI.Models.Location", "Location")
                        .WithMany("Orders")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheElectricCityAPI.Models.OrderItem", b =>
                {
                    b.HasOne("TheElectricCityAPI.Models.InventoryItem", "InventoryItem")
                        .WithMany("OrderItems")
                        .HasForeignKey("InventoryItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheElectricCityAPI.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
