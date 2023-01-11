using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database
{
    public class KnitterNotebookContext : DbContext
    {
        public KnitterNotebookContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Theme> Themes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.Property(x => x.Id).IsRequired();
                u.Property(x => x.Password).IsRequired().HasMaxLength(50);
                u.Property(x => x.Email).IsRequired().HasMaxLength(100);
                u.Property(x => x.Nickname).IsRequired().HasMaxLength(50);
                u.HasMany(x => x.Projects)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
            });

            modelBuilder.Entity<Project>(p =>
            {
                p.Property(x => x.Id).IsRequired();
                p.Property(x => x.User).IsRequired();
                p.Property(x => x.UserId).IsRequired();
            });

            modelBuilder.Entity<Theme>(t =>
            {
                t.Property(x => x.Id).IsRequired();
                t.Property(x => x.Name).IsRequired();
                t.HasMany(x => x.Users)
                .WithOne(c => c.Theme)
                .HasForeignKey(c => c.ThemeId);
            });
        }
    }
}
