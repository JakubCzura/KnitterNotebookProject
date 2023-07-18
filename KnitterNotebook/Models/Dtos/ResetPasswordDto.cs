namespace KnitterNotebook.Models.Dtos
{
    public record ResetPasswordDto(string EmailOrNickname, string NewPassword, string RepeatedNewPassword);
}