using FluentAssertions;
using KnitterNotebook.Database;

namespace KnitterNotebookTests.Database
{
    public class PasswordHasherTests
    {
        [Theory]
        [InlineData("password")]
        [InlineData("!12password")]
        [InlineData("!12passwor32")]
        [InlineData("!12pas32")]
        [InlineData("!12pas@@32")]
        public void HashPassword_ForValidData_ReturnsHashedPassword(string password)
        {
            //Act
            string hashedPassword = PasswordHasher.HashPassword(password);

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
            string hashedPassword = PasswordHasher.HashPassword(password);

            //Act
            bool result = PasswordHasher.VerifyPassword(password, hashedPassword);

            //Assert
            result.Should().BeTrue();
        }
    }
}