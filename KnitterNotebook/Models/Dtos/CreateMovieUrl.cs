namespace KnitterNotebook.Models.Dtos
{
    public class CreateMovieUrl
    {
        public CreateMovieUrl(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public string Title { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }
}