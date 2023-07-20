using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnitterNotebook.Validators;
using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;
using System.IO.Abstractions.TestingHelpers;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebookTests.HelpersForTesting;

namespace KnitterNotebookTests.Validators
{
    public class CreateSampleDtoValidatorTests
    {
        private readonly CreateSampleDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        public CreateSampleDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new CreateSampleDtoValidator(_databaseContext);
            SeedUsers();
        }
        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 },
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new CreateSampleDto("", 2, 3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto(null!, 2, 3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto(new string('K', 201), 2, 3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\user\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", -2, 3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 100001, 3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, -3, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 100001, 2, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 25, 0.09, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 101, 101, "cm", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "m", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, null!, "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, " ", "test", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", new string('K', 10001), 1, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", -1, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 0, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 10, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 1, "", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 1, "c:\\test\\test.txt", "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 1, "c:\\test\\test.jpg", "c:\\test\\test.txt") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, "cm", "description is ok", 20, "c:\\test\\test.jpg", "c:\\test\\test.jpg") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateSampleDto("Name", 2, 3, 2, "cm", "Description", 1, "c:\\test\\test.jpg", "c:\\test\\user\\test.jpg") };  
            yield return new object[] { new CreateSampleDto("Yarn Name", 1, 30, 1.5, "mm", "Description", 2, "c:\\test\\test.png", "c:\\test\\user\\test.png") };  
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task Validate_ForInvalidData_FailValidation(CreateSampleDto createSampleDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Fact]
        public async Task Validate_ForExistingImage_FailValidation()
        {
            //Arrange
            string destionationImagePath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "DirectoryForTests", "UserName", "ImageName.jpg");
            Directory.CreateDirectory(Path.GetDirectoryName(destionationImagePath)!);
            File.Create(destionationImagePath);

            CreateSampleDto createSampleDto = new("YarnName", 2, 2, 2, "cm", "Description", 1, "c:\\test\\ImageName.jpg", destionationImagePath);

            //Act
            var validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task Validate_ForValidData_PassValidation(CreateSampleDto createSampleDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}
