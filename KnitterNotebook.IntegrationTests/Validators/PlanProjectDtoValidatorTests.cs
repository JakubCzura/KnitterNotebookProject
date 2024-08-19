using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Validators;

public class PlanProjectDtoValidatorTests : IDisposable
{
    private readonly PlanProjectDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public PlanProjectDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new PlanProjectDtoValidator(_userService);
        SeedUsers();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedUsers()
    {
        List<User> users =
        [
            new User() { Nickname = "Nickname1", ThemeId = 1 },
            new User() { Nickname = "Nickname", ThemeId = 2 },
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    public static IEnumerable<object[]> ValidData()
    {
        yield return new object[] { new PlanProjectDto(
                                    "Knitting project",
                                    null,
                                    [new(2.5, NeedleSizeUnit.mm)],
                                    [new("My favourite yarn")],
                                    "Sample description",
                                    null,
                                    1) };
        yield return new object[] { new PlanProjectDto(
                                    "My project",
                                    DateTime.Today,
                                    [new(1, NeedleSizeUnit.cm), new(2, NeedleSizeUnit.mm)],
                                    [new("Cotton yarn")],
                                    null,
                                    null,
                                    1) };
        yield return new object[] { new PlanProjectDto(
                                    "Sample project",
                                    DateTime.Today.AddDays(2),
                                    [new(4, NeedleSizeUnit.cm)],
                                    [new("Woolen yarn")],
                                    "Description of my project",
                                    @"c:\users\user\files\projectPattern.pdf",
                                    2) };
    }

    [Theory]
    [MemberData(nameof(ValidData))]
    public async Task ValidateAsync_ForValidData_PassValidation(PlanProjectDto planProjectDto)
    {
        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars")]
    public async Task ValidateAsync_ForInvalidName_FailValidation(string name)
    {
        //Arrange
        PlanProjectDto planProjectDto = new(name, DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 1);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
    }

    public static IEnumerable<object[]> InvalidNeedlesData()
    {
        yield return new object[] { null! };
        yield return new object[] { new List<CreateNeedleDto>() };
    }

    [Theory]
    [MemberData(nameof(InvalidNeedlesData))]
    public async Task ValidateAsync_ForInvalidNeedles_FailValidation(List<CreateNeedleDto> createNeedleDtos)
    {
        //Arrange
        PlanProjectDto planProjectDto = new("Name", DateTime.Today, createNeedleDtos, Enumerable.Empty<CreateYarnDto>(), null, null, 1);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Needles);
    }

    public static IEnumerable<object[]> InvalidYarnsData()
    {
        yield return new object[] { null! };
        yield return new object[] { new List<CreateYarnDto>() };
    }

    [Theory]
    [MemberData(nameof(InvalidYarnsData))]
    public async Task ValidateAsync_ForInvalidYarns_FailValidation(List<CreateYarnDto> yarns)
    {
        //Arrange
        PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), yarns, null, null, 1);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Yarns);
    }

    [Fact]
    public async Task ValidateAsync_ForInvalidDescription_FailValidation()
    {
        //Arrange
        PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), new string('K', 301), null, 2);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public async Task ValidateAsync_ForInvalidSourcePatternPdfPath_FailValidation()
    {
        //Arrange
        PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, @"c:\users\user\files\file.jpeg", 2);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.SourcePatternPdfPath);
    }

    [Fact]
    public async Task ValidateAsync_ForInvalidUserId_FailValidation()
    {
        //Arrange
        //User with id 6 does not exist in SeedUsers()
        PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 6);

        //Act
        TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}