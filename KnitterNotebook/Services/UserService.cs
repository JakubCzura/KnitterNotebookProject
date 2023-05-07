using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class UserService : CrudService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IThemeRepository _themeRepository;

        public UserService(IUserRepository userRepository, IThemeRepository themeRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _themeRepository = themeRepository;
        }

        public async Task CreateAsync(RegisterUserDto registerUserDto)
        {
            User user = new()
            {
                Nickname = registerUserDto.Nickname,
                Email = registerUserDto.Email,
                Password = PasswordHasher.HashPassword(registerUserDto.Password),
            };
            await _userRepository.CreateAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            User user = await _userRepository.GetAsync(changePasswordDto.UserId);
            user.Password = PasswordHasher.HashPassword(changePasswordDto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto)
        {
            User user = await _userRepository.GetAsync(changeNicknameDto.UserId);
            user.Nickname = changeNicknameDto.Nickname;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            User user = await _userRepository.GetAsync(changeEmailDto.UserId);
            user.Email = changeEmailDto.Email;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangeThemeAsync(ChangeThemeDto changeThemeDto)
        {
            User user = await _userRepository.GetAsync(changeThemeDto.UserId);
            user.Theme = await _themeRepository.GetByNameAsync(changeThemeDto.ThemeName);
            await _userRepository.UpdateAsync(user);
        }
    }
}