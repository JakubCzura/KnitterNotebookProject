using FluentAssertions;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Views.UserControls;
using System.Windows.Controls;

namespace KnitterNotebook.UnitTests.Services
{
    public class WindowContentServiceTests
    {
        [WpfTheory]
        [InlineData(MainWindowContent.PlannedProjectsUserControl, typeof(PlannedProjectsUserControl))]
        [InlineData(MainWindowContent.ProjectsInProgressUserControl, typeof(ProjectsInProgressUserControl))]
        [InlineData(MainWindowContent.ProjectsUserControl, typeof(FinishedProjectsUserControl))]
        [InlineData(MainWindowContent.SamplesUserControl, typeof(SamplesUserControl))]
        [InlineData((MainWindowContent)999, typeof(SamplesUserControl))] // Unknown value
        public void ChooseMainWindowContent_ForChosenContent_ReturnsCorrectUserControl(MainWindowContent windowContent, Type expected)
        {
            // Arrange
            WindowContentService service = new();

            // Act
            UserControl result = service.ChooseMainWindowContent(windowContent);

            //Assert
            result.Should().BeOfType(expected);
        }

        [WpfTheory]
        [InlineData(SettingsWindowContent.UserSettingsUserControl, typeof(UserSettingsUserControl))]
        [InlineData(SettingsWindowContent.ThemeSettingsUserControl, typeof(ThemeSettingsUserControl))]
        [InlineData((SettingsWindowContent)999, typeof(UserSettingsUserControl))] // Unknown value
        public void ChooseSettingsWindowContent_ForChosenContent_ReturnsCorrectUserControl(SettingsWindowContent contentName, Type expected)
        {
            // Arrange
            WindowContentService service = new();

            // Act
            UserControl result = service.ChooseSettingsWindowContent(contentName);

            // Assert
            result.Should().BeOfType(expected);
        }
    }
}