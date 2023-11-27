using FluentAssertions;
using KnitterNotebook.Helpers.Extensions;

namespace KnitterNotebook.UnitTests.Helpers.Extensions;

public class ListExtensionTests
{
    [Fact]
    public void AddIfNotNullOrEmpty_ForNullList_DoesNotThrowException()
    {
        //Arrange
        List<string> list = null!;
        string? item = null;

        //Act
        Action action = () => list.AddIfNotNullOrEmpty(item);

        //Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void AddIfNotNullOrEmpty_ForValidItem_AddsItem()
    {
        //Arrange
        List<string> list = [];
        string? item = "item";

        //Act
        list.AddIfNotNullOrEmpty(item);

        //Assert
        list.Should().Contain(item);
    }

    [Theory]
    [InlineData(null), InlineData("")]
    public void AddIfNotNullOrEmpty_ForInvalidItem_DoesNotAddItem(string? item)
    {
        //Arrange
        List<string> list = [];

        //Act
        list.AddIfNotNullOrEmpty(item);

        //Assert
        list.Should().NotContain(item);
    }

    [Fact]
    public void AddRangeIfNotNullOrEmpty_ForNullList_DoesNotThrowException()
    {
        //Arrange
        List<string> list = null!;
        IEnumerable<string?> items = new List<string?>() { null, "item1", "item2" };

        //Act
        Action action = () => list.AddRangeIfNotNullOrEmpty(items);

        //Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void AddRangeIfNotNullOrEmpty_ForNullItems_DoesNotThrowException()
    {
        //Arrange
        List<string> list = [];
        IEnumerable<string?> items = null!;

        //Act
        Action action = () => list.AddRangeIfNotNullOrEmpty(items);

        //Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void AddRangeIfNotNullOrEmpty_ForValidItems_AddsItems()
    {
        //Arrange
        List<string> list = [];
        List<string> items = ["item1", "item2"];

        //Act
        list.AddRangeIfNotNullOrEmpty(items);

        //Assert
        list.Should().Contain(items);
    }
}