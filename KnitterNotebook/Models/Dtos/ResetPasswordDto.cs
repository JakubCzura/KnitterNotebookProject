namespace KnitterNotebook.Models.Dtos;

public record ResetPasswordDto(string Email, string Token, string NewPassword, string RepeatedNewPassword);