using FluentAssertions;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Services;

namespace KnitterNotebook.UnitTests.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService = new();

    [Fact]
    public void CreateResetPasswordToken_CreatesUniqueToken()
    {
        // Act
        List<string> tokens = [];
        for (int i = 0; i < 5; i++)
        {
            tokens.Add(_tokenService.CreateResetPasswordToken());
        }

        // Assert
        tokens.Distinct().Count().Should().Be(tokens.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CreateResetPasswordTokenExpirationDate_CreatesTokenExpirationDateInFuture(int daysToExpire)
    {
        // Act
        DateTime tokenExpirationDate = _tokenService.CreateResetPasswordTokenExpirationDate(daysToExpire);

        // Assert
        tokenExpirationDate.Should().BeAfter(DateTime.UtcNow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    public void CreateResetPasswordTokenExpirationDate_ForDaysToExpireAreLessThanOne_ThrowsTokenExpirationDateException(int daysToExpire)
    {
        // Act
        Action act = () => _tokenService.CreateResetPasswordTokenExpirationDate(daysToExpire);

        // Assert
        act.Should().Throw<TokenExpirationDateException>();
    }
}