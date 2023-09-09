using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class ThemeServiceTests
    {
        private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
        private readonly ThemeService _themeService;

        public ThemeServiceTests()
        {
            _themeService = new(_databaseContext);
            SeedThemes();
        }

        private void SeedThemes()
        {
            List<Theme> themes = new()
            {
                new() { Name = ApplicationTheme.Default },
                new() { Name = ApplicationTheme.Light },
                new() { Name = ApplicationTheme.Dark }
            };
            _databaseContext.Themes.AddRange(themes);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [InlineData(ApplicationTheme.Default), InlineData(ApplicationTheme.Light), InlineData(ApplicationTheme.Dark)]
        public async Task ThemeExistsAsync_ForExistingTheme_ReturnsTrue(ApplicationTheme themeName)
        {
            //Act
            bool result = await _themeService.ThemeExistsAsync(themeName);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ThemeExistsAsync_ForNotExistingTheme_ReturnsTrue()
        {
            //Assert
            ApplicationTheme themeName = (ApplicationTheme)999;

            //Act
            bool result = await _themeService.ThemeExistsAsync(themeName);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetThemeIdAsync_ForGivenThemeName_ReturnsThemeId()
        {
            //Assert
            ApplicationTheme themeName = ApplicationTheme.Light;

            //Act
            int? result = await _themeService.GetThemeIdAsync(themeName);

            //Assert
            result.Should().Be(2); //2 as Light theme has Id = 2 in SeedThemes()
        }

        [Fact]
        public async Task GetThemeIdAsync_ForNotExistingThemeName_ReturnsNull()
        {
            //Assert
            ApplicationTheme themeName = (ApplicationTheme)999;

            //Act
            int? result = await _themeService.GetThemeIdAsync(themeName);

            //Assert
            result.Should().BeNull();
        }
    }
}