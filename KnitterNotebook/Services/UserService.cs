﻿using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
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

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            User? user = await _databaseContext.Users.FindAsync(changePasswordDto.UserId);
            user!.Password = PasswordHasher.HashPassword(changePasswordDto.NewPassword);
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync(true);
        }

        public async Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto)
        {
            User? user = await _databaseContext.Users.FindAsync(changeNicknameDto.UserId);
            user!.Nickname = changeNicknameDto.Nickname;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            User? user = await _databaseContext.Users.FindAsync(changeEmailDto.UserId);
            user!.Email = changeEmailDto.Email;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ChangeThemeAsync(ChangeThemeDto changeThemeDto)
        {
            User? user = await _databaseContext.Users.FindAsync(changeThemeDto.UserId);
            user!.Theme = await _themeService.GetByNameAsync(changeThemeDto.ThemeName);
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }
    }
}