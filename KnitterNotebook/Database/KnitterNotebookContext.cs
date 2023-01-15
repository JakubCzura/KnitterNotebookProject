using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database
{
    public class KnitterNotebookContext : DbContext
    {
        public KnitterNotebookContext()
        {
        }

        public KnitterNotebookContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Theme> Themes { get; set; }

        AppSettings AppSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
            string appSettingsString = File.ReadAllText(appSettingsPath);
            AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString);

            optionsBuilder.UseSqlServer(AppSettings.KnitterNotebookConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Id).IsRequired();
                u.Property(x => x.Password).IsRequired().HasMaxLength(50);
                u.Property(x => x.Email).IsRequired().HasMaxLength(100);
                u.Property(x => x.Nickname).IsRequired().HasMaxLength(50);
                u.Property(x => x.ThemeId).IsRequired().HasDefaultValue(1);
                u.HasMany(x => x.Projects)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
            });

            modelBuilder.Entity<Project>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Id).IsRequired();
                //p.Property(x => x.User).IsRequired();
                //p.Property(x => x.UserId).IsRequired();
            });

            modelBuilder.Entity<Theme>(t =>
            {
                t.HasKey(x => x.Id);
                t.Property(x => x.Id).IsRequired();
                t.Property(x => x.Name).IsRequired();
                t.HasData(new Theme() { Id = 1, Name = "Default" }, new Theme() { Id = 2, Name = "Light" }, new Theme() { Id = 3, Name = "Dark" });
                t.HasMany(x => x.Users)
                .WithOne(c => c.Theme)
                .HasForeignKey(c => c.ThemeId);
            });
        }
    }
}
