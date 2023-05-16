﻿using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<MovieUrl> MovieUrls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
            //string appSettingsString = File.ReadAllText(appSettingsPath);
            //AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString)!;
            //optionsBuilder.UseSqlServer(AppSettings.KnitterNotebookConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Id).IsRequired();
                u.Property(x => x.Password).IsRequired();
                u.Property(x => x.Email).IsRequired().HasMaxLength(100);
                u.Property(x => x.Nickname).IsRequired().HasMaxLength(50);

                u.HasMany(x => x.Projects)
                 .WithOne(c => c.User);

                u.HasMany(x => x.MovieUrls)
                 .WithOne(c => c.User);
            });

            modelBuilder.Entity<Project>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Id).IsRequired();
                p.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Theme>(t =>
            {
                t.HasKey(x => x.Id);
                t.Property(x => x.Id).IsRequired();
                t.Property(x => x.Name).IsRequired();
                t.HasData(new Theme() { Id = 1, Name = "Default" },
                          new Theme() { Id = 2, Name = "Light" },
                          new Theme() { Id = 3, Name = "Dark" });
                t.HasMany(x => x.Users)
                 .WithOne(c => c.Theme)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<MovieUrl>(m =>
            {
                m.HasKey(x => x.Id);
                m.Property(x => x.Id).IsRequired();
                m.Property(x => x.Title).IsRequired();
                m.Property(x => x.Link).IsRequired();
            });
        }
    }
}