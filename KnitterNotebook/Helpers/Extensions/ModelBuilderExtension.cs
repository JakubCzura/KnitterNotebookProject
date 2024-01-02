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
            movieUrl.Property(movieUrl => movieUrl.Title)
                    .IsRequired()
                    .HasMaxLength(100);

            movieUrl.Property(movieUrl => movieUrl.Link)
                    .IsRequired();

            movieUrl.Property(movieUrl => movieUrl.Description)
                    .HasMaxLength(100);
        });

        modelBuilder.Entity<Sample>(sample =>
        {
            sample.Property(sample => sample.YarnName)
                  .IsRequired()
                  .HasMaxLength(200);
            
            sample.Property(sample => sample.LoopsQuantity)
                  .IsRequired()
                  .HasMaxLength(100000);
            
            sample.Property(sample => sample.RowsQuantity)
                  .IsRequired()
                  .HasMaxLength(100000);
           
            sample.Property(sample => sample.NeedleSize)
                  .IsRequired()
                  .HasMaxLength(100);
           
            sample.Property(sample => sample.Description)
                  .HasMaxLength(1000);
           
            sample.Property(sample => sample.NeedleSizeUnit)
                  .IsRequired()
                  .HasConversion<string>();
           
            sample.HasOne(sample => sample.Image)
                  .WithOne(sampleImage => sampleImage.Sample);
        });

        modelBuilder.Entity<SampleImage>(sampleImage =>
        {
            sampleImage.Property(sampleImage => sampleImage.Path)
                       .IsRequired();
        });

        modelBuilder.Entity<ProjectImage>(projectImage =>
        {
            projectImage.Property(projectImage => projectImage.DateTime)
                        .IsRequired();

            projectImage.Property(projectImage => projectImage.Path)
                        .IsRequired();
        });

        modelBuilder.Entity<PatternPdf>(patternPdf =>
        {
            patternPdf.Property(patternPdf => patternPdf.Path)
                      .IsRequired();
        });

        modelBuilder.Entity<Needle>(needle =>
        {
            needle.Property(needle => needle.Size)
                  .IsRequired()
                  .HasMaxLength(100);

            needle.Property(needle => needle.SizeUnit)
                  .IsRequired();
        });

        modelBuilder.Entity<Yarn>(yarn =>
        {
            yarn.Property(yarn => yarn.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        return modelBuilder;
    }
}