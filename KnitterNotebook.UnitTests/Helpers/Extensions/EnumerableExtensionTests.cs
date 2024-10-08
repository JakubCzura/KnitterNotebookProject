﻿using FluentAssertions;
using KnitterNotebook.Helpers.Extensions;
using System.Collections.ObjectModel;

namespace KnitterNotebook.UnitTests.Helpers.Extensions;

public class EnumerableExtensionTests
{
    public static IEnumerable<object[]> ToObservableCollection_ValidData()
    {
        yield return new object[] { new List<int> { 1, 2, 3 }, new ObservableCollection<int> { 1, 2, 3 } };
        yield return new object[] { new List<string> { "string1", "string2", "string3" }, new ObservableCollection<string> { "string1", "string2", "string3" } };
        yield return new object[] { new List<char> { 'k', 'j', 'r' }, new ObservableCollection<char> { 'k', 'j', 'r' } };
    }

    [Theory]
    [MemberData(nameof(ToObservableCollection_ValidData))]
    public void ToObservableCollection_ForValidData_ReturnsObservableCollection<T>(IEnumerable<T> enumerable, ObservableCollection<T> expected)
    {
        //Act
        ObservableCollection<T> result = enumerable.ToObservableCollection();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToObservableCollection_ForEmptyData_ReturnsEmptyObservableCollection()
    {
        //Arrange
        IEnumerable<int> enumerable = [];

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

    [Fact]
    public void NotNullAndHaveAnyElement_ForNullCollection_ReturnsFalse()
    {
        //Arrange
        List<int> list = null!;

        //Act
        bool result = list.NotNullAndHaveAnyElement();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void NotNullAndHaveAnyElement_ForEmptyCollection_ReturnsFalse()
    {
        //Arrange
        List<int> list = [];

        //Act
        bool result = list.NotNullAndHaveAnyElement();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void NotNullAndHaveAnyElement_ForCollectionWithElements_ReturnsTrue()
    {
        //Arrange
        List<int> list = [1, 2, 3];

        //Act
        bool result = list.NotNullAndHaveAnyElement();

        //Assert
        result.Should().BeTrue();
    }
}