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

namespace KnitterNotebookTests.Validators
{
    public class CreateSampleDtoValidatorTests
    {
        private readonly CreateSampleDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        public CreateSampleDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new CreateSampleDtoValidator(_databaseContext);
            SeedUsers();
        }
        private static string CreateUniqueDatabaseName => "TestDb" + DateTime.Now.Ticks.ToString();

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
            yield return new object[] { new CreateSampleDto("test", -2, -3 , 2, "cm", "test", 1, "c:", "c:")};
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
    }
}
