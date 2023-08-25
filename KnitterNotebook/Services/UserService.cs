using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class UserService : CrudService<User>, IUserService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IThemeService _themeService;
        private readonly IPasswordService _passwordService;

        public UserService(DatabaseContext databaseContext, IThemeService themeService, IPasswordService passwordService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _themeService = themeService;
            _passwordService = passwordService;
        }

        public async Task<bool> IsNicknameTakenAsync(string nickname) => await _databaseContext.Users.AnyAsync(x => x.Nickname == nickname);

        public async Task<bool> IsEmailTakenAsync(string email) => await _databaseContext.Users.AnyAsync(x => x.Email == email);

        public async Task<bool> UserExistsAsync(int id) => await _databaseContext.Users.AnyAsync(x => x.Id == id);

        /// <returns>User object if found in database otherwise null</returns>
        public new async Task<UserDto?> GetAsync(int id)
        {
            User? user = await _databaseContext.Users
                          .Include(x => x.Theme)
                          .FirstOrDefaultAsync(x => x.Id == id);

            return user is null ? null : new UserDto(user);
        }

        public async Task<int?> LogInAsync(LogInDto logInDto)
        {
            User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == logInDto.Email);

            return user is not null && _passwordService.VerifyPassword(logInDto.Password, user.Password)
                    ? user.Id
                    : null;
        }

        public async Task CreateAsync(RegisterUserDto registerUserDto)
        {
            User user = new()
            {
                Nickname = registerUserDto.Nickname,
                Email = registerUserDto.Email,
                Password = _passwordService.HashPassword(registerUserDto.Password),
                Theme = await _themeService.GetByNameAsync(ApplicationTheme.Default)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.ThemeWithNameNotFound(ApplicationTheme.Default.ToString()))
            };
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
        }

        /// <returns>Nickname if user was found otherwise null</returns>
        public async Task<string?> GetNicknameAsync(int id) => (await _databaseContext.Users.FindAsync(id))?.Nickname;

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            User user = await _databaseContext.Users.FindAsync(changePasswordDto.UserId)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(changePasswordDto.UserId));

            user.Password = _passwordService.HashPassword(changePasswordDto.NewPassword);
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync(true);
        }

        public async Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto)
        {
            User user = await _databaseContext.Users.FindAsync(changeNicknameDto.UserId)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(changeNicknameDto.UserId));

            user.Nickname = changeNicknameDto.Nickname;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            User user = await _databaseContext.Users.FindAsync(changeEmailDto.UserId)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(changeEmailDto.UserId));

            user.Email = changeEmailDto.Email;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ChangeThemeAsync(ChangeThemeDto changeThemeDto)
        {
            User user = await _databaseContext.Users.FindAsync(changeThemeDto.UserId)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(changeThemeDto.UserId));

            user.Theme = await _themeService.GetByNameAsync(changeThemeDto.ThemeName)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.ThemeWithNameNotFound(changeThemeDto.ThemeName.ToString()));

            _themeService.ReplaceTheme(changeThemeDto.ThemeName, user.Theme.Name);

            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            User user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == resetPasswordDto.Email)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithEmailNotFound(resetPasswordDto.Email));

            user.Password = _passwordService.HashPassword(resetPasswordDto.NewPassword);
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public void LogOut() => Environment.Exit(0);
    }
}