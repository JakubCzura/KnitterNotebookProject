using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Database;

public class DatabaseContext : DbContext
{
    public const string DatabaseConnectionStringKey = "KnitterNotebookConnectionString";

    public DatabaseContext(DbContextOptions options) : base(options)
    {
        if (Database.IsRelational())
        {
            Database.Migrate();
        }
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
        => modelBuilder.ConfigureDatabaseEntities();
}