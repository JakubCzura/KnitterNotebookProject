﻿// <auto-generated />
using System;
using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KnitterNotebook.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KnitterNotebook.Models.Entities.MovieUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("MovieUrls");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Needle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<double>("Size")
                        .HasMaxLength(100)
                        .HasColumnType("float");

                    b.Property<int>("SizeUnit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Needles");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.PatternPdf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("PatternPdfs");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.ProjectImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectImages");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Sample", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("LoopsQuantity")
                        .HasMaxLength(100000)
                        .HasColumnType("int");

                    b.Property<double>("NeedleSize")
                        .HasMaxLength(100)
                        .HasColumnType("float");

                    b.Property<string>("NeedleSizeUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RowsQuantity")
                        .HasMaxLength(100000)
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("YarnName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.SampleImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SampleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SampleId")
                        .IsUnique();

                    b.ToTable("SampleImages");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Themes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Default"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Light"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Dark"
                        });
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordResetTokenExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Yarn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Yarns");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.MovieUrl", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.User", null)
                        .WithMany("MovieUrls")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Needle", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Project", "Project")
                        .WithMany("Needles")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.PatternPdf", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Project", "Project")
                        .WithOne("PatternPdf")
                        .HasForeignKey("KnitterNotebook.Models.Entities.PatternPdf", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Project", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.User", null)
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.ProjectImage", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Project", "Project")
                        .WithMany("ProjectImages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Sample", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.User", null)
                        .WithMany("Samples")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.SampleImage", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Sample", "Sample")
                        .WithOne("Image")
                        .HasForeignKey("KnitterNotebook.Models.Entities.SampleImage", "SampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.User", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Theme", "Theme")
                        .WithMany("Users")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Yarn", b =>
                {
                    b.HasOne("KnitterNotebook.Models.Entities.Project", "Project")
                        .WithMany("Yarns")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Project", b =>
                {
                    b.Navigation("Needles");

                    b.Navigation("PatternPdf");

                    b.Navigation("ProjectImages");

                    b.Navigation("Yarns");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Sample", b =>
                {
                    b.Navigation("Image");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.Theme", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("KnitterNotebook.Models.Entities.User", b =>
                {
                    b.Navigation("MovieUrls");

                    b.Navigation("Projects");

                    b.Navigation("Samples");
                });
#pragma warning restore 612, 618
        }
    }
}
