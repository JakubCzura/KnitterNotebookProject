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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}