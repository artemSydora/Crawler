﻿// <auto-generated />
using System;
using Crawler.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Crawler.Repository.Migrations
{
    [DbContext(typeof(CrawlerDbContext))]
    partial class CrawlerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Crawler.Entities.Models.DetailDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("InSitemap")
                        .HasColumnType("bit");

                    b.Property<bool>("InWebsite")
                        .HasColumnType("bit");

                    b.Property<int>("ResponseTimeMs")
                        .HasColumnType("int");

                    b.Property<int?>("TestDTOId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("Id");

                    b.HasIndex("TestDTOId");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("Crawler.Entities.Models.TestDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StartPageUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Crawler.Entities.Models.DetailDTO", b =>
                {
                    b.HasOne("Crawler.Entities.Models.TestDTO", null)
                        .WithMany("Details")
                        .HasForeignKey("TestDTOId");
                });

            modelBuilder.Entity("Crawler.Entities.Models.TestDTO", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
