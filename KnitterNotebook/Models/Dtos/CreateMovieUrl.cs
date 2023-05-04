namespace KnitterNotebook.Models.Dtos
{
    public class CreateMovieUrl
    {
        public CreateMovieUrl(string title, string link, User user)
        {
            Title = title;
            Link = link;
            User = user;
        }

        public string Title { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        public User User = new();
    }
}