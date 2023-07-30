using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
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

        public UserService(DatabaseContext databaseContext, IThemeService themeService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _themeService = themeService;
        }

        /// <returns>User object if found in database otherwise null</returns>
        public new async Task<UserDto?> GetAsync(int id)
        {
            User? user = await _databaseContext.Users
                          .Include(x => x.MovieUrls)
                          .Include(x => x.Projects)
                          .Include(x => x.Theme)
                          .Include(x => x.Samples).ThenInclude(x => x.Image)
                          .Include(x => x.Projects).ThenInclude(x => x.Needles)
                          .Include(x => x.Projects).ThenInclude(x => x.Yarns)
                          .FirstOrDefaultAsync(x => x.Id == id);

            return user is null ? null : new UserDto(user.Id, user.Nickname, user.Email, user.Projects, user.Samples, user.MovieUrls, user.Theme);
        }

        public async Task CreateAsync(RegisterUserDto registerUserDto)
        {
            User user = new()
            {
                Nickname = registerUserDto.Nickname,
                Email = registerUserDto.Email,
                Password = PasswordHasher.HashPassword(registerUserDto.Password),
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

            user.Password = PasswordHasher.HashPassword(changePasswordDto.NewPassword);
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

            user.Theme = await _themeService.GetByNameAsync(changeThemeDto.ThemeName)!;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            User user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == resetPasswordDto.EmailOrNickname || x.Nickname == resetPasswordDto.EmailOrNickname)
                        ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithNicknameOrEmailNotFound(resetPasswordDto.EmailOrNickname));

            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public void LogOut() => Environment.Exit(0);
    }
}