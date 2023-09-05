using FluentAssertions;
using KnitterNotebook.Services;

namespace KnitterNotebook.UnitTests.Services
{
    public class PasswordServiceTests
    {
        [Theory]
        [InlineData("password")]
        [InlineData("!12password")]
        [InlineData("!12passwor32")]
        [InlineData("!12pas32")]
        [InlineData("!12pas@@32")]
        public void HashPassword_ForValidData_ReturnsHashedPassword(string password)
        {
            //Arrange
            PasswordService passwordService = new();

            //Act
            string hashedPassword = passwordService.HashPassword(password);

            //Assert
            //Bcrypt is used to hash so hashed password should start with $2
            hashedPassword.Should().StartWith("$2");
            hashedPassword.Should().NotBe(password);
        }

        [Theory]
        [InlineData("password")]
        [InlineData("pass@1123word")]
        [InlineData("paKs@1123wWord")]
        [InlineData("p@13dd23wWord")]
        public void VerifyPassword_ForValidData_ReturnsTrue(string password)
        {
            //Arrange
            PasswordService passwordService = new();

            //Act
            string hashedPassword = passwordService.HashPassword(password);
            bool result = passwordService.VerifyPassword(password, hashedPassword);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void HashPassword_ForNull_ThrowsArgumentNullException()
        {
            //Arrange
            PasswordService passwordService = new();
            string password = null!;

            //Act
            Action action = () => passwordService.HashPassword(password);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void VerifyPassword_ForNullPassword_ThrowsArgumentNullException()
        {
            //Arrange
            PasswordService passwordService = new();
            string password = null!;
            string hashedPassword = "dd22#@!FF";
            //Act
            Action action = () => passwordService.VerifyPassword(password, hashedPassword);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void VerifyPassword_ForNullHashedPassword_ThrowsArgumentNullException()
        {
            //Arrange
            PasswordService passwordService = new();
            string password = "dd22#@!FF";
            string hashedPassword = null!;
            //Act
            Action action = () => passwordService.VerifyPassword(password, hashedPassword);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}