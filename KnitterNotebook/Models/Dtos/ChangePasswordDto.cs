namespace KnitterNotebook.Models.Dtos
{
    public class ChangePasswordDto
    {
        public ChangePasswordDto(int userId, string newPassword, string confirmPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public int UserId { get; set; }

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}