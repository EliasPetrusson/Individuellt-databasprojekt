﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using School;

#nullable disable

namespace ELIASPETRUSSON.Migrations
{
    [DbContext(typeof(SkolaDbContext))]
    [Migration("20240109144816_AddAnstallningsdatumAndAvdelning")]
    partial class AddAnstallningsdatumAndAvdelning
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("School.SkolaDbContext+Betyg", b =>
                {
                    b.Property<int>("BetygId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BetygId"));

                    b.Property<DateTime>("BetygDatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Betygsgrad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ElevId")
                        .HasColumnType("int");

                    b.Property<int>("KursId")
                        .HasColumnType("int");

                    b.HasKey("BetygId");

                    b.HasIndex("ElevId");

                    b.HasIndex("KursId");

                    b.ToTable("BetygSet");
                });

            modelBuilder.Entity("School.SkolaDbContext+Elev", b =>
                {
                    b.Property<int>("ElevId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ElevId"));

                    b.Property<string>("Klass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Namn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Personnummer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ElevId");

                    b.ToTable("Elever");
                });

            modelBuilder.Entity("School.SkolaDbContext+Kurs", b =>
                {
                    b.Property<int>("KursId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KursId"));

                    b.Property<string>("Kursnamn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KursId");

                    b.ToTable("Kurser");
                });

            modelBuilder.Entity("School.SkolaDbContext+Personal", b =>
                {
                    b.Property<int>("PersonalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonalId"));

                    b.Property<DateTime>("Anställningsdatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Avdelning")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Befattning")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Namn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonalId");

                    b.ToTable("PersonalSet");
                });

            modelBuilder.Entity("School.SkolaDbContext+Betyg", b =>
                {
                    b.HasOne("School.SkolaDbContext+Elev", "BetygElev")
                        .WithMany()
                        .HasForeignKey("ElevId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("School.SkolaDbContext+Kurs", "BetygKurs")
                        .WithMany()
                        .HasForeignKey("KursId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BetygElev");

                    b.Navigation("BetygKurs");
                });
#pragma warning restore 612, 618
        }
    }
}
