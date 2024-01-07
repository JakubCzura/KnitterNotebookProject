using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Models.Settings;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class UserService(DatabaseContext databaseContext,
                         IThemeService themeService,
                         IPasswordService passwordService,
                         ITokenService tokenService,
                         IConfiguration configuration,
                         SharedResourceViewModel sharedResourceViewModel) : CrudService<User>(databaseContext), IUserService
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IThemeService _themeService = themeService;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;
    private readonly SharedResourceViewModel _sharedResourceViewModel = sharedResourceViewModel;

    public async Task<bool> IsNicknameTakenAsync(string nickname) 
        => await _databaseContext.Users.AsNoTracking()
                                       .AnyAsync(x => x.Nickname == nickname);

    public async Task<bool> IsEmailTakenAsync(string email) 
        => await _databaseContext.Users.AsNoTracking()
                                       .AnyAsync(x => x.Email == email);

    public async Task<bool> ArePasswordResetTokenAndExpirationDateValidAsync(string token)
    {
        User? user = await _databaseContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PasswordResetToken == token);

        return user is not null
               && user.PasswordResetToken is not null
               && user.PasswordResetToken == token
               && user.PasswordResetTokenExpirationDate.HasValue
               && user.PasswordResetTokenExpirationDate.Value.ToUniversalTime() >= DateTime.UtcNow;
    }

    public async Task<bool> UserExistsAsync(int id)
        => await _databaseContext.Users.AsNoTracking()
                                       .AnyAsync(x => x.Id == id);

    /// <summary>
    /// Returns instance of user from database
    /// </summary>
    /// <param name="id">Id of user</param>
    /// <returns>User's data from database or null if user doesn't exist</returns>
    public new async Task<UserDto?> GetAsync(int id)
    {
        User? user = await _databaseContext.Users.AsNoTracking()
                                                 .Include(x => x.Theme)
                                                 .FirstOrDefaultAsync(x => x.Id == id);

        return user is null ? null : new UserDto(user);
    }

    /// <summary>
    /// Logs in user if email and password are correct
    /// </summary>
    /// <param name="logInDto">Data to log in user</param>
    /// <returns>User's id if user logs in, otherwise null</returns>
    public async Task<int?> LogInAsync(LogInDto logInDto)
    {
        User? user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == logInDto.Email);

        return user is not null && _passwordService.VerifyPassword(logInDto.Password, user.Password)
                ? user.Id
                : null;
    }

    /// <summary>
    /// Adds new user to database
    /// </summary>
    /// <param name="registerUserDto">Dto with user information</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="EntityNotFoundException">If default Theme entity doesn't exists in database</exception>
    /// <exception cref="NullReferenceException">If <paramref name="registerUserDto"/> is null</exception>
    public async Task<int> CreateAsync(RegisterUserDto registerUserDto)
    {
        User user = new()
        {
            Nickname = registerUserDto.Nickname,
            Email = registerUserDto.Email,
            Password = _passwordService.HashPassword(registerUserDto.Password),
            ThemeId = await _themeService.GetThemeIdAsync(ApplicationTheme.Default)
                    ?? throw new EntityNotFoundException(ExceptionsMessages.ThemeWithNameNotFound(ApplicationTheme.Default.ToString()))
        };
        await _databaseContext.Users.AddAsync(user);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<string?> GetNicknameAsync(int id) 
        => (await _databaseContext.Users.FindAsync(id))?.Nickname;

    /// <summary>
    /// Changes user's password and saves it to database
    /// </summary>
    /// <param name="changePasswordDto">Data to change</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="changePasswordDto"/> is null</exception>
    public async Task<int> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        => await _databaseContext.Users.Where(x => x.Id == changePasswordDto.UserId)
                                       .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Password, _passwordService.HashPassword(changePasswordDto.NewPassword)));

    /// <summary>
    /// Changes user's nickname and saves it to database
    /// </summary>
    /// <param name="changeNicknameDto">Data to change</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="changeNicknameDto"/> is null</exception>
    public async Task<int> ChangeNicknameAsync(ChangeNicknameDto changeNicknameDto)
        => await _databaseContext.Users.Where(x => x.Id == changeNicknameDto.UserId)
                                       .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Nickname, changeNicknameDto.Nickname));

    /// <summary>
    /// Changes user's email and saves it to database
    /// </summary>
    /// <param name="changeEmailDto">Data to change</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="changeEmailDto"/> is null</exception>
    public async Task<int> ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        => await _databaseContext.Users.Where(x => x.Id == changeEmailDto.UserId)
                                       .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Email, changeEmailDto.Email));

    /// <summary>
    /// Changes theme and saves it to database. Updates user interface with new theme
    /// </summary>
    /// <param name="changeThemeDto">Data to update</param>
    /// <returns>Quantity of entities saved to database or 0 if user doesn't exist in database</returns>
    /// <exception cref="EntityNotFoundException">If theme doesn't exists in database</exception>
    /// <exception cref="NullReferenceException">If <paramref name="changeThemeDto"/> is null</exception>
    public async Task<int> ChangeThemeAsync(ChangeThemeDto changeThemeDto)
    {
        int themeId = await _themeService.GetThemeIdAsync(changeThemeDto.ThemeName)
                    ?? throw new EntityNotFoundException(ExceptionsMessages.ThemeWithNameNotFound(changeThemeDto.ThemeName.ToString()));

        User? user = await _databaseContext.Users.Include(x => x.Theme).FirstOrDefaultAsync(x => x.Id == changeThemeDto.UserId);
        if (user == null)
        {
            return 0;
        }

        _themeService.ReplaceTheme(changeThemeDto.ThemeName, user.Theme.Name);

        return _databaseContext.Users.Where(x => x.Id == changeThemeDto.UserId)
                                     .ExecuteUpdate(setters => setters.SetProperty(x => x.ThemeId, themeId));
    }

    /// <summary>
    /// Changes user's password and saves it to database
    /// </summary>
    /// <param name="resetPasswordDto">Data to update</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="resetPasswordDto"/> is null</exception>"
    public async Task<int> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        => await _databaseContext.Users.Where(x => x.Email == resetPasswordDto.Email)
                                       .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Password, _passwordService.HashPassword(resetPasswordDto.NewPassword)));

    /// <summary>
    /// Updates user's password reset token and its expiration date
    /// </summary>
    /// <param name="userEmail">User's email</param>
    /// <returns>User's password reset token and token's expiration date</returns>
    /// <exception cref="EntityNotFoundException">If user doesn't exist in database</exception>
    public async Task<(string, DateTime)> UpdatePasswordResetTokenAsync(string userEmail)
    {
        TokensSettings? tokensSettings = _configuration.GetSection(TokensSettings.SectionKey).Get<TokensSettings>();
        var tokenWithExpirationDate = new
        {
            Token = _tokenService.CreateResetPasswordToken(),
            ExpirationDate = _tokenService.CreateResetPasswordTokenExpirationDate(tokensSettings?.ResetPasswordTokenExpirationDays ?? 1)
        };

        return await _databaseContext.Users
                                     .Where(x => x.Email == userEmail)
                                     .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.PasswordResetToken, tokenWithExpirationDate.Token)
                                                                           .SetProperty(x => x.PasswordResetTokenExpirationDate, tokenWithExpirationDate.ExpirationDate)) > 0
               ? (tokenWithExpirationDate.Token, tokenWithExpirationDate.ExpirationDate)
               : throw new EntityNotFoundException(ExceptionsMessages.UserWithEmailNotFound(userEmail));
    }

    /// <summary>
    /// Logs out user, deletes user's unnecessary files and closes application
    /// </summary>
    public void LogOut()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        // This garbage collector's operations prevent errors while deleting files : "Cannot access file because it is being used by another process"
        _sharedResourceViewModel.FilesToDelete.ForEach(x => { if (File.Exists(x)) File.Delete(x); });
        Environment.Exit(0);
    }
}