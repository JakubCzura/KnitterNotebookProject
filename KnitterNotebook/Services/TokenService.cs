using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Security.Cryptography;

namespace KnitterNotebook.Services
{
    public class TokenService : ITokenService
    {
        public string CreateResetPasswordToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        /// <summary>
        /// Creates token expiration date based on <paramref name="daysToExpire"/> and <see cref="DateTime.UtcNow"/>.
        /// </summary>
        /// <param name="daysToExpire">Quantity of days when token is valid</param>
        /// <returns>Token necessary for resetting password</returns>
        /// <exception cref="TokenExpirationDateException"></exception>
        public DateTime CreateResetPasswordTokenExpirationDate(int daysToExpire)
        {
            if(daysToExpire < 1)
            {
                throw new TokenExpirationDateException(ExceptionsMessages.TokenExpirationDateTooEarly(daysToExpire));
            }
            return DateTime.UtcNow.AddDays(daysToExpire);
        }
    }
}