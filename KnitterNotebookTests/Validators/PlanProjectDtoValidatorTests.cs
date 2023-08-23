using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class PlanProjectDtoValidatorTests
    {
        private readonly PlanProjectDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        public PlanProjectDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object);
            _validator = new PlanProjectDtoValidator(_userService);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 },
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new PlanProjectDto(null!, DateTime.Today.AddDays(-1), null!, null!, null, null, 0) };
            yield return new object[] { new PlanProjectDto(string.Empty, DateTime.Today.AddDays(-2), Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 0) };
            yield return new object[] { new PlanProjectDto(string.Empty, DateTime.Today.AddDays(-3), Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), "Description", @"c:\users\user\files\file.txt", 4) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), "Description", @"c:\users\user\files\file.pdf", 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, @"c:\users\user\files\file.pdf", 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 2) };
            yield return new object[] { new PlanProjectDto(new string('K', 101), DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), new string('K', 301), null, 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), "Description", null, 4) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), new List<CreateYarnDto>() { new CreateYarnDto("Cotton yarn") }, "Description", null, 1) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, new List<CreateNeedleDto>() { new CreateNeedleDto(1, NeedleSizeUnit.cm), new CreateNeedleDto(2, NeedleSizeUnit.mm) }, Enumerable.Empty<CreateYarnDto>(), "Description", null, 2) };
            yield return new object[] { new PlanProjectDto("Name", DateTime.Today, new List<CreateNeedleDto>() { new CreateNeedleDto(1, NeedleSizeUnit.cm), new CreateNeedleDto(2, NeedleSizeUnit.mm) }, new List<CreateYarnDto>() { new CreateYarnDto("Cotton yarn") }, "Description", null, 4) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new PlanProjectDto(
                                        "My project",
                                        DateTime.Today,
                                        new List<CreateNeedleDto>(){ new CreateNeedleDto(1, NeedleSizeUnit.cm), new CreateNeedleDto (2, NeedleSizeUnit.mm)},
                                        new List<CreateYarnDto>(){ new CreateYarnDto("Cotton yarn") },
                                        null,
                                        null,
                                        1) };
            yield return new object[] { new PlanProjectDto(
                                        "Sample project",
                                        DateTime.Today.AddDays(2),
                                        new List<CreateNeedleDto>(){ new CreateNeedleDto(4, NeedleSizeUnit.cm) },
                                        new List<CreateYarnDto>(){ new CreateYarnDto("Woolen yarn") },
                                        "Description of my project",
                                        @"c:\users\user\files\projectPattern.pdf",
                                        2) };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task Validate_ForInvalidData_FailValidation(PlanProjectDto planProjectDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task Validate_ForValidData_PassValidation(PlanProjectDto planProjectDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}