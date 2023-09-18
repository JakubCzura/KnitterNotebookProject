using System;

namespace KnitterNotebook.Services.Interfaces;

public interface ITokenService
{
    public string CreateResetPasswordToken();

    public DateTime CreateResetPasswordTokenExpirationDate(int daysToExpire);
}