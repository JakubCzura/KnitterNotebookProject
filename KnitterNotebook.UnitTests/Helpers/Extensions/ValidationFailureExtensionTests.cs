using FluentAssertions;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;

namespace KnitterNotebook.UnitTests.Helpers.Extensions;

public class ValidationFailureExtensionTests
{
    public static TheoryData<IEnumerable<ValidationFailure>, string> ValidData => new()
    {
        { new List<ValidationFailure> { new("propertyName", "message") }, "message" },
        { new List<ValidationFailure> { new("propertyName", "message1"), new("propertyName", "message2") }, $"message1{Environment.NewLine}message2" }
    };

    [Theory]
    [MemberData(nameof(ValidData))]
    public void GetMessagesAsString_ForValidData_ReturnsString(IEnumerable<ValidationFailure> validationFailures, string expected)
    {
        //Act
        string result = validationFailures.GetMessagesAsString();

        //Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetMessagesAsString_ForEmptyData_ReturnsEmptyString()
    {
        //Arrange
        IEnumerable<ValidationFailure> validationFailures = [];

        //Act
        string result = validationFailures.GetMessagesAsString();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetMessagesAsString_ForNullData_ThrowsArgumentNullException()
    {
        //Arrange
        IEnumerable<ValidationFailure> validationFailures = null!;

        //Act
        Action action = () => validationFailures.GetMessagesAsString();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }
}