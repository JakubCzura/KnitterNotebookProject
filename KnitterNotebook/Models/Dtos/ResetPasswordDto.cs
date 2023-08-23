namespace KnitterNotebook.Models.Dtos
{
    public record ResetPasswordDto(string Email, string NewPassword, string RepeatedNewPassword);
}