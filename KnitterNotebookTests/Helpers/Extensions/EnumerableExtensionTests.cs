using FluentAssertions;
using System.Collections.ObjectModel;

namespace KnitterNotebook.Helpers.Extensions
{
    public class EnumerableExtensionTests
    {
        [Fact]
        public void ToObservableCollection_ForValidData_ReturnsObservableCollection()
        {
            //Arrange
            List<int> enumerable1 = new() { 1, 2, 3 };
            List<string> enumerable2 = new() { "string1", "string2", "string3" };
            List<char> enumerable3 = new() { 'k', 'j', 'r' };

            ObservableCollection<int> expected1 = new() { 1, 2, 3 };
            ObservableCollection<string> expected2 = new() { "string1", "string2", "string3" };
            ObservableCollection<char> expected3 = new() { 'k', 'j', 'r' };

            //Act
            ObservableCollection<int> result1 = enumerable1.ToObservableCollection();
            ObservableCollection<string> result2 = enumerable2.ToObservableCollection();
            ObservableCollection<char> result3 = enumerable3.ToObservableCollection();

            //Assert
            result1.Should().BeEquivalentTo(expected1);
            result2.Should().BeEquivalentTo(expected2);
            result3.Should().BeEquivalentTo(expected3);
        }

        [Fact]
        public void ToObservableCollection_ForEmptyData_ReturnsEmptyObservableCollection()
        {
            //Arrange
            IEnumerable<int> enumerable = Enumerable.Empty<int>();

            //Act
            ObservableCollection<int> result = enumerable.ToObservableCollection();

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToObservableCollection_ForNullData_ThrowsArgumentNullException()
        {
            //Arrange
            List<int> enumerable = null!;

            //Act
            Action action = () => enumerable.ToObservableCollection();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}