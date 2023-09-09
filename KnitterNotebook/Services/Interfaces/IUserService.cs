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

        Task<int> CreateAsync(RegisterUserDto data);

        new Task<UserDto?> GetAsync(int id);

        Task<string?> GetNicknameAsync(int id);

        Task<int> ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto);

        Task<int> ChangeEmailAsync(ChangeEmailDto changeEmailDto);

        Task<int> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        Task<int> ChangeThemeAsync(ChangeThemeDto changeThemeDto);

        Task<int> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<(string, DateTime)> UpdatePasswordResetTokenStatusAsync(string userEmail);

        Task<bool> ArePasswordResetTokenAndExpirationDateValidAsync(string token);

        void LogOut();
    }
}