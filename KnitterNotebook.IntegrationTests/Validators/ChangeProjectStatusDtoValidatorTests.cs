using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnitterNotebookTests.IntegrationTests.Validators
{
    public class ChangeProjectStatusDtoValidatorTests
    {
        private readonly ChangeProjectStatusDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly ProjectService _projectService;
        private readonly Mock<IUserService> _iUserServiceMock = new();

        public ChangeProjectStatusDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _projectService = new(_databaseContext, _iUserServiceMock.Object);
            _validator = new ChangeProjectStatusDtoValidator(_projectService);
            SeedProjects();
        }

        private void SeedProjects()
        {
            List<Project> projects = new()
            {
                new Project() { Id = 1, },
                new Project() { Id = 2, },
            };
            _databaseContext.Projects.AddRange(projects);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeProjectStatusDto(1, ProjectStatusName.Planned) };
            yield return new object[] { new ChangeProjectStatusDto(1, ProjectStatusName.InProgress) };
            yield return new object[] { new ChangeProjectStatusDto(2, ProjectStatusName.Finished) };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangeProjectStatusDto changeProjectStatusDto)
        {
            // Act
            TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        public async Task ValidateAsync_ForInvalidProjectId_FailValidation(int id)
        {
            //Arrange
            ChangeProjectStatusDto changeProjectStatusDto = new(id, ProjectStatusName.Planned);

            // Act
            TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidProjectStatusName_FailValidation()
        {
            //Arrange
            ChangeProjectStatusDto changeProjectStatusDto = new(1, (ProjectStatusName)100);

            // Act
            TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.ProjectStatus);
        }
    }
}