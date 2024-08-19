using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Validators;

public class CreateMovieUrlDtoValidatorTests : IDisposable
{
    private readonly CreateMovieUrlDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public CreateMovieUrlDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new CreateMovieUrlDtoValidator(_userService);
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
            new User() { Nickname = "Name1", ThemeId = 1 },
            new User() { Nickname = "Nickname2", ThemeId = 1 }
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    public static TheoryData<CreateMovieUrlDto> InvalidData => new()
    {
        { new CreateMovieUrlDto(null!, "https://youtube.pl", null, 1) },
        { new CreateMovieUrlDto("Movie", null!, string.Empty, 1) },
        { new CreateMovieUrlDto("Funny movie", "https://urltestmovie/96.pl", null, -1) },
        { new CreateMovieUrlDto(string.Empty, "https://testurltestmovie/96.pl", null, 2) },
        { new CreateMovieUrlDto("Scary movie", string.Empty, null, 2) },
        { new CreateMovieUrlDto("Scary movie", string.Empty, null, 0) },
        { new CreateMovieUrlDto("Scary movie", string.Empty, new string('K', 101), 3) }
    };

    public static TheoryData<CreateMovieUrlDto> ValidData => new()
    {
        { new CreateMovieUrlDto("Title", "https://youtube.pl", "Description", 1) },
        { new CreateMovieUrlDto("Movie", "https://movieurl/21.pl", null, 2) }
    };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public async Task ValidateAsync_ForInvalidData_FailValidation(CreateMovieUrlDto createMovieUrlDto)
    {
        //Act
        TestValidationResult<CreateMovieUrlDto> validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

        //Assert
        validationResult.ShouldHaveAnyValidationError();
    }

    [Theory]
    [MemberData(nameof(ValidData))]
    public async Task ValidateAsync_ForValidData_PassValidation(CreateMovieUrlDto createMovieUrlDto)
    {
        //Act
        TestValidationResult<CreateMovieUrlDto> validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}