using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Security.Cryptography;

namespace KnitterNotebook.Services;

public class TokenService : ITokenService
{
    /// <summary>
    /// Creates random token for resetting password
    /// </summary>
    /// <returns>Random token</returns>
    public string CreateResetPasswordToken() 
        => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

    /// <summary>
    /// Creates token expiration date based on <paramref name="daysToExpire"/> and <see cref="DateTime.UtcNow"/>.
    /// </summary>
    /// <param name="daysToExpire">Quantity of days when token is valid</param>
    /// <returns>DateTime object when token necessary for resetting password expires</returns>
    /// <exception cref="TokenExpirationDateException">If <paramref name="daysToExpire"/> is less than 1</exception>
    public DateTime CreateResetPasswordTokenExpirationDate(int daysToExpire)
    {
        if (daysToExpire < 1)
        {
            throw new TokenExpirationDateException(ExceptionsMessages.TokenExpirationDateTooEarly(daysToExpire));
        }
        return DateTime.UtcNow.AddDays(daysToExpire);
    }
}