using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IUserService : ICrudService<User>
    {
        Task<bool> IsNicknameTakenAsync(string nickname);

        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> UserExistsAsync(int id);

        Task<int?> LogInAsync(LogInDto logInDto);

        Task CreateAsync(RegisterUserDto data);

        new Task<UserDto?> GetAsync(int id);

        Task<string?> GetNicknameAsync(int id);

        Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto);

        Task ChangeEmailAsync(ChangeEmailDto changeEmailDto);

        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        Task ChangeThemeAsync(ChangeThemeDto changeThemeDto);

        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<(string, DateTime)> UpdatePasswordResetTokenStatusAsync(string userEmail);

        void LogOut();
    }
}