namespace KnitterNotebook.Models.Dtos
{
    public class CreateMovieUrlDto
    {
        public CreateMovieUrlDto(string title, string link, int userId)
        {
            Title = title;
            Link = link;
            UserId = userId;
        }

        public string Title { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        public int UserId;
    }
}