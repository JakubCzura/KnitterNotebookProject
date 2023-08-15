namespace KnitterNotebook.Models
{
    public class SampleImage
    {
        public SampleImage(string path)
        {
            Path = path;
        }

        public int Id { get; set; }

        public string Path { get; set; } = string.Empty;

        public virtual Sample Sample { get; set; } = new();

        public int SampleId { get; set; }
    }
}