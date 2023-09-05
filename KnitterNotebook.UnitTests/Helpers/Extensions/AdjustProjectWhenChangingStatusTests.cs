using FluentAssertions;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.UnitTests.Helpers.Extensions
{
    public class AdjustProjectWhenChangingStatusTests
    {
        public static IEnumerable<object[]> NewProjectStatusData()
        {
            yield return new object[]
            {
                new Project() { ProjectStatus = ProjectStatusName.Planned, StartDate = null, EndDate = null },
                ProjectStatusName.InProgress,
                new Project() { ProjectStatus = ProjectStatusName.InProgress, StartDate = DateTime.Today, EndDate = null },
            };
            yield return new object[]
            {
                new Project() { ProjectStatus = ProjectStatusName.InProgress, StartDate = DateTime.Today, EndDate = null },
                ProjectStatusName.Planned,
                new Project() { ProjectStatus = ProjectStatusName.Planned, StartDate = null, EndDate = null },
            };
            yield return new object[]
            {
                new Project() { ProjectStatus = ProjectStatusName.InProgress, StartDate = DateTime.Today, EndDate = null },
                ProjectStatusName.Finished,
                new Project() { ProjectStatus = ProjectStatusName.Finished, StartDate = DateTime.Today, EndDate = DateTime.Today },
            };
            yield return new object[]
            {
                new Project() { ProjectStatus = ProjectStatusName.Finished, StartDate = DateTime.Today, EndDate = DateTime.Today },
                ProjectStatusName.InProgress,
                new Project() { ProjectStatus = ProjectStatusName.InProgress, StartDate = DateTime.Today, EndDate = null },
            };
        }

        [Theory]
        [MemberData(nameof(NewProjectStatusData))]
        public void AdjustProjectWhenChangingStatus_ForNewStatus_AdjustsProject(Project project, ProjectStatusName projectNewStatusName, Project expected)
        {
            //Act
            project.AdjustProjectWhenChangingStatus(projectNewStatusName);

            //Assert
            project.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustProjectWhenChangingStatus_ForNullProject_ThrowsArgumentNullException()
        {
            //Arrange
            Project project = null!;

            //Act
            Action action = () => project.AdjustProjectWhenChangingStatus(ProjectStatusName.Planned);

            //Assert
            action.Should().Throw<NullReferenceException>();
        }
    }
}