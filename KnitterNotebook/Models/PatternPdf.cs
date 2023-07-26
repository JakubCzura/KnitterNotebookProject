namespace KnitterNotebook.Models
{
    public class PatternPdf
    {
        public PatternPdf(string path)
        {
            Path = path;
        }

        public int Id { get; set; }

        public string Path { get; set; } = string.Empty;

        public virtual Project Project { get; set; } = new();

        public int ProjectId { get; set; }
    }
}
