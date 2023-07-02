﻿namespace KnitterNotebook.Models.Dtos
{
    public class ResetPasswordDto
    {
        public ResetPasswordDto(string emailOrNickname, string newPassword, string repeatedNewPassword)
        {
            EmailOrNickname = emailOrNickname;
            NewPassword = newPassword;
            RepeatedNewPassword = repeatedNewPassword;
        }

        public string EmailOrNickname { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RepeatedNewPassword { get; set; } = string.Empty;
    }
}