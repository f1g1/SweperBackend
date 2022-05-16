﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using SweperBackend.Data;

#nullable disable

namespace SweperBackend.Migrations
{
    [DbContext(typeof(SweperBackendContext))]
    [Migration("20220516205602_CascadeImages")]
    partial class CascadeImages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SweperBackend.Data.InitialForm", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("PriceCategory")
                        .HasColumnType("int");

                    b.Property<int>("SpaceCategory")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("InitialForm");
                });

            modelBuilder.Entity("SweperBackend.Data.RentItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastLogin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Point>("Location")
                        .HasColumnType("geography");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Radius")
                        .HasColumnType("int");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<int>("Surface")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RentItem");
                });

            modelBuilder.Entity("SweperBackend.Data.RentItemImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RentItemId")
                        .HasColumnType("int");

                    b.Property<string>("Timestamp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RentItemId");

                    b.ToTable("RentItemImage");
                });

            modelBuilder.Entity("SweperBackend.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastLogin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SweperBackend.Data.UserPreferredLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Point>("Location")
                        .HasColumnType("geography");

                    b.Property<int>("Radius")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserPreferredLocation");
                });

            modelBuilder.Entity("SweperBackend.Data.UserRentItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateInteraction")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateViewd")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Liked")
                        .HasColumnType("bit");

                    b.Property<int>("RentItemId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RentItemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRentItem");
                });

            modelBuilder.Entity("SweperBackend.Data.InitialForm", b =>
                {
                    b.HasOne("SweperBackend.Data.User", "User")
                        .WithOne("InitialForm")
                        .HasForeignKey("SweperBackend.Data.InitialForm", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SweperBackend.Data.RentItem", b =>
                {
                    b.HasOne("SweperBackend.Data.User", "User")
                        .WithMany("RentItems")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SweperBackend.Data.RentItemImage", b =>
                {
                    b.HasOne("SweperBackend.Data.RentItem", "RentItem")
                        .WithMany("RentItemImages")
                        .HasForeignKey("RentItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("RentItem");
                });

            modelBuilder.Entity("SweperBackend.Data.UserPreferredLocation", b =>
                {
                    b.HasOne("SweperBackend.Data.User", "User")
                        .WithMany("PrefferedLocations")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SweperBackend.Data.UserRentItem", b =>
                {
                    b.HasOne("SweperBackend.Data.RentItem", "RentItem")
                        .WithMany()
                        .HasForeignKey("RentItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SweperBackend.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("RentItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SweperBackend.Data.RentItem", b =>
                {
                    b.Navigation("RentItemImages");
                });

            modelBuilder.Entity("SweperBackend.Data.User", b =>
                {
                    b.Navigation("InitialForm");

                    b.Navigation("PrefferedLocations");

                    b.Navigation("RentItems");
                });
#pragma warning restore 612, 618
        }
    }
}