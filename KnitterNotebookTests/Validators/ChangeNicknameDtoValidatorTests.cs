using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class ChangeNicknameDtoValidatorTests
    {
        private readonly ChangeNicknameDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public ChangeNicknameDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _validator = new ChangeNicknameDtoValidator(_userService);
            SeedUsers();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeNicknameDto(1, "NewNick1") };
            yield return new object[] { new ChangeNicknameDto(2, "TestNewNick") };
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Nickname = "Nick1"},
                new User() { Id = 2, Nickname = "TestNick"},
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation(int userId)
        {
            //Arrange
            ChangeNicknameDto changeNicknameDto = new(userId, "Nick1");

            //Act
            TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("InvalidTooLongNicknameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
        public async Task ValidateAsync_ForInvalidNickname_FailValidation(string nickname)
        {
            //Arrange
            ChangeNicknameDto changeNicknameDto = new(1, nickname);

            //Act
            TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Nickname);
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangeNicknameDto changeNicknameDto)
        {
            //Act
            TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}