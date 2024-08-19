using FluentAssertions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.UnitTests.Helpers.Filters;

public class SamplesFilterTests
{
    [Theory]
    [InlineData(6, NeedleSizeUnit.mm, null, NeedleSizeUnit.mm, true)]
    [InlineData(6, NeedleSizeUnit.mm, null, NeedleSizeUnit.cm, true)]
    [InlineData(6, NeedleSizeUnit.cm, null, NeedleSizeUnit.cm, true)]
    [InlineData(6, NeedleSizeUnit.cm, null, NeedleSizeUnit.mm, true)]
    [InlineData(2.4, NeedleSizeUnit.mm, 2.4, NeedleSizeUnit.mm, true)]
    [InlineData(2.5, NeedleSizeUnit.cm, 2.5, NeedleSizeUnit.cm, true)]
    [InlineData(2.5, NeedleSizeUnit.cm, 3.5, NeedleSizeUnit.cm, false)]
    [InlineData(2.5, NeedleSizeUnit.cm, 3.5, NeedleSizeUnit.mm, false)]
    [InlineData(2.5, NeedleSizeUnit.mm, 3.5, NeedleSizeUnit.cm, false)]
    public void FilterByNeedleSize_ForGivenSample_ReturnsBoolean(double entityNeedleSize, NeedleSizeUnit entityNeedleSizeUnit, double? needleSize, NeedleSizeUnit needleSizeUnit, bool expected)
    {
        //Arrange
        BasicSampleDto sample = new(new Sample() { NeedleSize = entityNeedleSize, NeedleSizeUnit = entityNeedleSizeUnit });

        //Act
        bool result = SamplesFilter.FilterByNeedleSize<BasicSampleDto>(sample, needleSize, needleSizeUnit);

        //Assert
        expected.Should().Be(result);
    }
}