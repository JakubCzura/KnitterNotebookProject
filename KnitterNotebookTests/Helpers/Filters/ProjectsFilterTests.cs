using FluentAssertions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Entities;

namespace KnitterNotebookTests.Helpers.Filters
{
    public class ProjectsFilterTests
    {
        public static IEnumerable<object[]> ProjectsWithGivenNamesComparison()
        {
            yield return new object[]
            {
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name1" },
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another PROJECT 2" },
                    new Project() { Id= 4, Name = "Project1" },
                    new Project() { Id= 5, Name = "Testing name" },
                    new Project() { Id= 5, Name = "Project" },
                },
                "Project",
                ProjectsFilter.NamesComparison.Contains,
                new List<Project>
                {
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another PROJECT 2" },
                    new Project() { Id= 4, Name = "Project1" },
                    new Project() { Id= 5, Name = "Project" },
                },
            };
            yield return new object[]
            {
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another project 2" },
                    new Project() { Id= 4, Name = "Name" },
                },
                "Name",
                ProjectsFilter.NamesComparison.Contains,
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 4, Name = "Name" },
                },
            };
            yield return new object[]
            {
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 2, Name = "Another name" },
                    new Project() { Id= 3, Name = "Another name 2" },
                    new Project() { Id= 4, Name = "Name" },
                    new Project() { Id= 5, Name = "Name" },
                },
                "Name",
                ProjectsFilter.NamesComparison.Equals,
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 4, Name = "Name" },
                    new Project() { Id= 5, Name = "Name" },
                },
            };
        }

        public static IEnumerable<object[]> ProjectsWithDefaultNamesComparison()
        {
            yield return new object[]
            {
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name1" },
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another PROJECT 2" },
                    new Project() { Id= 4, Name = "Project1" },
                    new Project() { Id= 5, Name = "Testing name" },
                    new Project() { Id= 5, Name = "Project" },
                },
                "Project",
                new List<Project>
                {
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another PROJECT 2" },
                    new Project() { Id= 4, Name = "Project1" },
                    new Project() { Id= 5, Name = "Project" },
                },
            };
            yield return new object[]
            {
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 2, Name = "Another project" },
                    new Project() { Id= 3, Name = "Another project 2" },
                    new Project() { Id= 4, Name = "Name" },
                },
                "Name",
                new List<Project>
                {
                    new Project() { Id= 1, Name = "Name" },
                    new Project() { Id= 4, Name = "Name" },
                },
            };
        }

        [Theory]
        [MemberData(nameof(ProjectsWithGivenNamesComparison))]
        public void FilterByName_ForProjectsWithGivenNamesComparison_ReturnsFilteredData(IEnumerable<Project> projects, string projectName, ProjectsFilter.NamesComparison namesComparison, IEnumerable<Project> expected)
        {
            //Act
            projects = ProjectsFilter.FilterByName(projects, projectName, namesComparison);

            //Assert
            expected.Should().BeEquivalentTo(projects);
        }

        [Theory]
        [MemberData(nameof(ProjectsWithDefaultNamesComparison))]
        public void FilterByName_ForProjectsWithDefaultNamesComparison_ReturnsFilteredData(IEnumerable<Project> projects, string projectName, IEnumerable<Project> expected)
        {
            //Act
            projects = ProjectsFilter.FilterByName(projects, projectName);

            //Assert
            expected.Should().BeEquivalentTo(projects);
        }

        [Fact]
        public void FilterByName_ForNullData_ThrowsArgumentNullException()
        {
            //Arrange
            IEnumerable<Project> projects = null!;

            //Act
            Action act = () => projects = ProjectsFilter.FilterByName(projects, "project");

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FilterByName_ForEmptyData_ReturnsEmptyData()
        {
            //Arrange
            IEnumerable<Project> projects = Enumerable.Empty<Project>();

            //Act
            projects = ProjectsFilter.FilterByName(projects, "projects");

            //Assert
            projects.Should().BeEmpty();
        }
    }
}