using KnitterNotebook.Services.Interfaces;
using System;
using System.Security.Cryptography;

namespace KnitterNotebook.Services
{
    public class TokenService : ITokenService
    {
        public string CreateResetPasswordToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        public DateTime CreateResetPasswordTokenExpirationDate(int daysToExpire) => DateTime.UtcNow.AddDays(daysToExpire);
    }
}