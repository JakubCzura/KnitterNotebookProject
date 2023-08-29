﻿using FluentValidation;
using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class CreateMovieUrlDtoValidatorTests
    {
        private readonly IValidator<CreateMovieUrlDto> _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();

        public CreateMovieUrlDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object);
            _validator = new CreateMovieUrlDtoValidator(_userService);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 }
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new CreateMovieUrlDto(null!, "https://youtube.pl", null, 1) };
            yield return new object[] { new CreateMovieUrlDto("Movie", null!, string.Empty, 1) };
            yield return new object[] { new CreateMovieUrlDto("Funny movie", "https://urltestmovie/96.pl", null, -1) };
            yield return new object[] { new CreateMovieUrlDto(string.Empty, "https://testurltestmovie/96.pl", null, 2) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, null, 2) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, null, 0) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, new string('K', 101), 3) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateMovieUrlDto("Title", "https://youtube.pl", "Description", 1) };
            yield return new object[] { new CreateMovieUrlDto("Movie", "https://movieurl/21.pl", null, 2) };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            TestValidationResult<CreateMovieUrlDto> validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            TestValidationResult<CreateMovieUrlDto> validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}