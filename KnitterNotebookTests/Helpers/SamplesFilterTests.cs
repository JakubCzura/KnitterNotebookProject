﻿using FluentAssertions;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace KnitterNotebookTests.Helpers
{
    public class SamplesFilterTests
    {
        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[]
            {
                new List<Sample>
                { 
                    new Sample(1, "YarnName1", 10, 10, 2, "mm", "Description", new User()),
                    new Sample(2, "YarnName2", 10, 10, 1, "mm", "Description", new User()),
                    new Sample(3, "YarnName3", 10, 10, 2, "mm", "Description", new User()),
                    new Sample(4, "YarnName4", 10, 10, 2, "cm", "Description", new User()), 
                    new Sample(5, "YarnName5", 10, 10, 3, "cm", "Description", new User()), 
                    new Sample(6, "YarnName6", 10, 10, 2, "mm", "Description", new User()) 
                },
                2,
                "mm",
                new List<Sample>
                {
                    new Sample(1, "YarnName1", 10, 10, 2, "mm", "Description", new User()),
                    new Sample(3, "YarnName3", 10, 10, 2, "mm", "Description", new User()),
                    new Sample(6, "YarnName6", 10, 10, 2, "mm", "Description", new User())
                }
            };
            yield return new object[]
            {
                new List<Sample>
                {
                    new Sample(1, "YarnName1", 10, 10, 3, "cm", "Description", new User()),
                    new Sample(2, "YarnName2", 10, 10, 1, "mm", "Description", new User()),
                    new Sample(3, "YarnName3", 10, 10, 3, "cm", "Description", new User()),
                    new Sample(4, "YarnName4", 10, 10, 3, "mm", "Description", new User()),
                    new Sample(5, "YarnName5", 10, 10, 5, "cm", "Description", new User()),
                    new Sample(6, "YarnName6", 10, 10, 3, "cm", "Description", new User())
                },
                3,
                "cm",
                new List<Sample>
                {
                    new Sample(1, "YarnName1", 10, 10, 3, "cm", "Description", new User()),
                    new Sample(3, "YarnName3", 10, 10, 3, "cm", "Description", new User()),
                    new Sample(6, "YarnName6", 10, 10, 3, "cm", "Description", new User())
                }
            };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void FilterByNeedleSize_ForValidData_ReturnsFilteredData(IEnumerable<Sample> samples, double needleSize, string needleSizeUnit, IEnumerable<Sample> result)
        {
            //Act
            samples = samples.FilterByNeedleSize(needleSize, needleSizeUnit);

            //Assert
            result.Should().BeEquivalentTo(samples);
        }

        [Fact]
        public void FilterByNeedleSize_ForNullData_ThrowsArgumentNullException()
        {
            //Arrange
            IEnumerable<Sample> samples = null!;

            //Act
            Action act = () => samples = samples.FilterByNeedleSize(2, "cm");

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FilterByNeedleSize_ForEmptyData_ReturnsEmptyData()
        {
            //Arrange
            ObservableCollection<Sample> samples = new ();

            //Act
            samples = samples.FilterByNeedleSize(2, "cm");

            //Assert
            samples.Should().BeEmpty();
        }
    }
}
