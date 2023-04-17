using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace KnitterNotebook.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IThemeRepository _themeRepository;

        public UserService(IUserRepository userRepository, IThemeRepository themeRepository)
        {
            _userRepository = userRepository;
            _themeRepository = themeRepository;
        }

        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> Get(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task Add(RegisterUserDto registerUserDto)
        {
            User user = new()
            {
                Nickname = registerUserDto.Nickname,
                Email = registerUserDto.Email,
                Password = PasswordHasher.HashPassword(registerUserDto.Password),
            };
            await _userRepository.Add(user);
        }

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            User user = await _userRepository.Get(changePasswordDto.UserId);
            user.Password = PasswordHasher.HashPassword(changePasswordDto.NewPassword);
            await _userRepository.Update(user);
        }

        public async Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto)
        {
            User user = await _userRepository.Get(changeNicknameDto.UserId);
            user.Nickname = changeNicknameDto.Nickname;
            await _userRepository.Update(user);
        }

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            User user = await _userRepository.Get(changeEmailDto.UserId);
            user.Email = changeEmailDto.Email;
            await _userRepository.Update(user);
        }

        public async Task ChangeThemeAsync(ChangeThemeDto changeThemeDto)
        {
            User user = await _userRepository.Get(changeThemeDto.UserId);
            user.Theme = await _themeRepository.GetByNameAsync(changeThemeDto.ThemeName);
            await _userRepository.Update(user);
        }
    }
}