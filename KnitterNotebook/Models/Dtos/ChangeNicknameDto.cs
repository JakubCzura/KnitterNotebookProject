namespace KnitterNotebook.Models.Dtos
{
    public class ChangeNicknameDto
    {
        public ChangeNicknameDto(int userId, string nickname)
        {
            UserId = userId;
            Nickname = nickname;
        }

        public int UserId { get; set; }

        public string Nickname { get; set; } = string.Empty;
    }
}