using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<MovieUrl> MovieUrls { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleImage> SampleImages { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<PatternPdf> PatternPdfs { get; set; }
        public DbSet<Needle> Needles { get; set; }
        public DbSet<Yarn> Yarns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>(t =>
            {
                t.Property(x => x.Name).IsRequired().HasConversion<string>();
                t.HasData(new Theme() { Id = 1, Name = ApplicationTheme.Default },
                          new Theme() { Id = 2, Name = ApplicationTheme.Light },
                          new Theme() { Id = 3, Name = ApplicationTheme.Dark });
                t.HasMany(x => x.Users)
                 .WithOne(c => c.Theme)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(u =>
            {
                u.Property(x => x.Password).IsRequired();
                u.Property(x => x.Email).IsRequired().HasMaxLength(100);
                u.Property(x => x.Nickname).IsRequired().HasMaxLength(50);

                u.HasMany(x => x.Projects)
                 .WithOne(c => c.User);

                u.HasMany(x => x.MovieUrls)
                 .WithOne(c => c.User);

                u.HasMany(x => x.Samples)
                 .WithOne(c => c.User);
            });

            modelBuilder.Entity<Project>(p =>
            {
                p.Property(x => x.Name).IsRequired().HasMaxLength(100);

                p.HasMany(x => x.Yarns)
                 .WithOne(c => c.Project);

                p.HasMany(x => x.Needles)
                 .WithOne(c => c.Project);

                p.HasOne(x => x.PatternPdf)
                 .WithOne(c => c.Project);
            });

            modelBuilder.Entity<MovieUrl>(m =>
            {
                m.Property(x => x.Title).IsRequired();
                m.Property(x => x.Link).IsRequired();
                m.Property(x => x.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<Sample>(m =>
            {
                m.Property(x => x.YarnName).IsRequired();
                m.Property(x => x.LoopsQuantity).IsRequired();
                m.Property(x => x.RowsQuantity).IsRequired();
                m.Property(x => x.NeedleSize).IsRequired();
                m.Property(x => x.NeedleSizeUnit).IsRequired().HasConversion<string>();
                m.HasOne(x => x.Image)
                 .WithOne(c => c.Sample);
            });

            modelBuilder.Entity<SampleImage>(m =>
            {
                m.Property(x => x.Path).IsRequired();
            });

            modelBuilder.Entity<ProjectImage>(m =>
            {
                m.Property(x => x.Path).IsRequired();
            });

            modelBuilder.Entity<PatternPdf>(m =>
            {
            });

            modelBuilder.Entity<Needle>(m =>
            {
            });

            modelBuilder.Entity<Yarn>(m =>
            {
            });
        }
    }
}