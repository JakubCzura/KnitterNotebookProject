using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<User> Get(int id);

        Task Add(RegisterUserDto registerUserDto);

        Task Update(User user);

        Task ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto);
        
        Task ChangeEmailAsync(ChangeEmailDto changeEmailDto);

        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        Task ChangeThemeAsync(ChangeThemeDto changeThemeDto);

        Task Delete(int id);
    }
}
