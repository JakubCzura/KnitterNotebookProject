using FluentAssertions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;

namespace KnitterNotebookTests.UnitTests.Helpers.Filters
{
    public class ProjectsFilterTests
    {
        [Theory]
        [InlineData("ProjectName", "ProjectName", ProjectsFilter.NamesComparison.Contains, true)]
        [InlineData("ProjectName", "Project", ProjectsFilter.NamesComparison.Contains, true)]
        [InlineData("ProjectName", "ProjectName", ProjectsFilter.NamesComparison.Equals, true)]
        [InlineData("ProjectName", "ProjectName", (ProjectsFilter.NamesComparison)999, false)]
        [InlineData("ProjectName", "AnotherProjectName", ProjectsFilter.NamesComparison.Contains, false)]
        [InlineData("ProjectName", "AnotherProjectName", ProjectsFilter.NamesComparison.Equals, false)]
        [InlineData("ProjectName", "AnotherProjectName", (ProjectsFilter.NamesComparison)999, false)]
        public void FilterByName_ForProjectWithGivenNamesComparison_ReturnsBoolean(string entityProjectName, string filterProjectName, ProjectsFilter.NamesComparison namesComparison, bool expected)
        {
            //Arrange
            BasicProjectDto project = new(new Project() { Name = entityProjectName });

            //Act
            bool result = ProjectsFilter.FilterByName<BasicProjectDto>(project, filterProjectName, namesComparison);

            //Assert
            expected.Should().Be(result);
        }

        [Theory]
        [InlineData("ProjectName", "Project", true)]
        [InlineData("ProjectName", "AnotherProject", false)]
        public void FilterByName_ForProjectWithDefaultNamesComparison_ReturnsBoolean(string entityProjectName, string filterProjectName, bool expected)
        {
            //Arrange
            BasicProjectDto project = new(new Project() { Name = entityProjectName });

            //Act
            bool result = ProjectsFilter.FilterByName<BasicProjectDto>(project, filterProjectName);

            //Assert
            expected.Should().Be(result);
        }
    }
}