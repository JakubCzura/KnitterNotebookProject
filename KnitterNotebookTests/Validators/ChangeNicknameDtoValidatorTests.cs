using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebookTests.Validators
{
    public class ChangeNicknameDtoValidatorTests
    {
        private readonly ChangeNicknameDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;

        public ChangeNicknameDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new ChangeNicknameDtoValidator(_databaseContext);
            SeedUsers();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ChangeNicknameDto(0, null!) };
            yield return new object[] { new ChangeNicknameDto(1, null!) };
            yield return new object[] { new ChangeNicknameDto(2, null!) };
            yield return new object[] { new ChangeNicknameDto(0, string.Empty) };
            yield return new object[] { new ChangeNicknameDto(1, string.Empty) };
            yield return new object[] { new ChangeNicknameDto(0, "8") };
            yield return new object[] { new ChangeNicknameDto(0, " ") };
            yield return new object[] { new ChangeNicknameDto(0, "     ") };
            yield return new object[] { new ChangeNicknameDto(2, " ") };
            yield return new object[] { new ChangeNicknameDto(1, "x#") };
            yield return new object[] { new ChangeNicknameDto(1, "1#") };
            yield return new object[] { new ChangeNicknameDto(1, "1 #") };
            yield return new object[] { new ChangeNicknameDto(1, "Nick1") };
            yield return new object[] { new ChangeNicknameDto(2, "Nick2") };
            yield return new object[] { new ChangeNicknameDto(2, "MoreThan50CharsMoreThan50CharsMoreThan50CharsMoreThan50CharsMoreThan50CharsMoreThan50CharsMoreThan50Chars") };
            yield return new object[] { new ChangeNicknameDto(90, "Nick333") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeNicknameDto(1, "NewNick1") };
            yield return new object[] { new ChangeNicknameDto(2, "NewNick2") };
            yield return new object[] { new ChangeNicknameDto(3, "NewNick3") };
        }

        private static string CreateUniqueDatabaseName => "TestDb" + DateTime.Now.Ticks.ToString();

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Nickname = "Nick1"},
                new User() { Id = 2, Nickname = "Nick2"},
                new User() { Id = 3, Nickname = "Nick3"}
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(ChangeNicknameDto changeNicknameDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeNicknameDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangeNicknameDto changeNicknameDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeNicknameDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}