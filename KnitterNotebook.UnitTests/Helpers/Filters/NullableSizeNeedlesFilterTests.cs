using FluentAssertions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models;

namespace KnitterNotebook.UnitTests.Helpers.Filters
{
    public class NullableSizeNeedlesFilterTests
    {
        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[]
            {
                new NullableSizeNeedle[]
                {
                    new NullableSizeNeedle() { Size = 1 },
                    new NullableSizeNeedle() { Size = 1.2 },
                    new NullableSizeNeedle() { Size = null },
                    new NullableSizeNeedle() { Size = 0 },
                    new NullableSizeNeedle() { Size = -1 },
                    new NullableSizeNeedle() { Size = 1.2 },
                    new NullableSizeNeedle() { Size = 3.21 },
                    new NullableSizeNeedle() { Size = 5.21 },
                },
                new NullableSizeNeedle[]
                {
                    new NullableSizeNeedle() { Size = 1 },
                    new NullableSizeNeedle() { Size = 1.2 },
                    new NullableSizeNeedle() { Size = 1.2 },
                    new NullableSizeNeedle() { Size = 3.21 },
                    new NullableSizeNeedle() { Size = 5.21 },
                },
            };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void GetNeedlesWithPositiveSizeValue_ForValidData_ReturnsFilteredData(NullableSizeNeedle[] needles, NullableSizeNeedle[] expected)
        {
            //Act
            IEnumerable<NullableSizeNeedle> result = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(needles);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetNeedlesWithPositiveSizeValue_ForNullData_ThrowsArgumentNullException()
        {
            //Act
            Action act = () => NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetNeedlesWithPositiveSizeValue_ForEmptyData_ReturnsEmptyData()
        {
            //Act
            IEnumerable<NullableSizeNeedle> needles = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue();

            //Assert
            needles.Should().BeEmpty();
        }
    }
}