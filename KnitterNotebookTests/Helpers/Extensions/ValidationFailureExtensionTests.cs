using FluentAssertions;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;

namespace KnitterNotebookTests.Helpers.Extensions
{
    public class ValidationFailureExtensionTests
    {
        [Fact]
        public void GetMessagesAsString_ForEmptyData_ReturnsEmptyString()
        {
            //Arrange
            IEnumerable<ValidationFailure> validationFailures = Enumerable.Empty<ValidationFailure>();

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
}