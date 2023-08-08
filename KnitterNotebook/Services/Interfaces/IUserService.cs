using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IUserService : ICrudService<User>
    {
        Task<int?> LogInAsync(LogInDto logInDto);

        Task CreateAsync(RegisterUserDto data);

        new Task<UserBasicDto?> GetAsync(int id);

        Task<string?> GetNicknameAsync(int id);

        Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto);

        Task ChangeEmailAsync(ChangeEmailDto changeEmailDto);

        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        Task ChangeThemeAsync(ChangeThemeDto changeThemeDto);

        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        void LogOut();
    }
}