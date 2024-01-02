using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Helpers.Extensions;

public static class ModelBuilderExtension
{
    public static ModelBuilder ConfigureDatabaseEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.Property(user => user.Password)
                .IsRequired();
           
            user.Property(user => user.Email)
                .IsRequired();
            
            user.Property(user => user.Nickname)
                .IsRequired()
                .HasMaxLength(50);

            user.HasMany(user => user.Projects)
                .WithOne()
                .HasForeignKey(project => project.UserId);

            user.HasMany(user => user.MovieUrls)
                .WithOne()
                .HasForeignKey(movieUrl => movieUrl.UserId);

            user.HasMany(user => user.Samples)
                .WithOne()
                .HasForeignKey(sample => sample.UserId);
        });

        modelBuilder.Entity<Theme>(theme =>
        {
            theme.HasMany(theme => theme.Users)
                 .WithOne(user => user.Theme)
                 .OnDelete(DeleteBehavior.NoAction);

            theme.Property(theme => theme.Name)
                 .IsRequired()
                 .HasConversion<string>();
             
            theme.HasData(new Theme() { Id = 1, Name = ApplicationTheme.Default },
                          new Theme() { Id = 2, Name = ApplicationTheme.Light },
                          new Theme() { Id = 3, Name = ApplicationTheme.Dark });
        });

        modelBuilder.Entity<Project>(project =>
        {
            project.Property(project => project.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            project.Property(project => project.Description)
                   .HasMaxLength(300);

            project.HasMany(project => project.Yarns)
                   .WithOne(yarn => yarn.Project);

            project.HasMany(project => project.Needles)
                   .WithOne(needle => needle.Project);

            project.HasOne(project => project.PatternPdf)
                   .WithOne(patternPdf => patternPdf.Project);
        });

        modelBuilder.Entity<MovieUrl>(movieUrl =>
        {
            movieUrl.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(100);

            movieUrl.Property(x => x.Link)
                    .IsRequired();

            movieUrl.Property(x => x.Description)
                    .HasMaxLength(100);
        });

        modelBuilder.Entity<Sample>(m =>
        {
            m.Property(x => x.YarnName).IsRequired().HasMaxLength(200);
            m.Property(x => x.LoopsQuantity).IsRequired().HasMaxLength(100000);
            m.Property(x => x.RowsQuantity).IsRequired().HasMaxLength(100000);
            m.Property(x => x.NeedleSize).IsRequired().HasMaxLength(100);
            m.Property(x => x.Description).HasMaxLength(1000);
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
            m.Property(x => x.DateTime).IsRequired();
            m.Property(x => x.Path).IsRequired();
        });

        modelBuilder.Entity<PatternPdf>(m =>
        {
            m.Property(x => x.Path).IsRequired();
        });

        modelBuilder.Entity<Needle>(m =>
        {
            m.Property(x => x.Size).IsRequired().HasMaxLength(100);
            m.Property(x => x.SizeUnit).IsRequired();
        });

        modelBuilder.Entity<Yarn>(m =>
        {
            m.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        return modelBuilder;
    }
}