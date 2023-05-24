namespace KnitterNotebook.Models
{
    public class Image
    {
        public Image(string path)
        {
            Path = path;
        }

        public int Id { get; set; }

        public string Path { get; set; } = string.Empty;

        public virtual Sample Sample { get; set; } = new();

        public int SampleId { get; set; }
    }
}