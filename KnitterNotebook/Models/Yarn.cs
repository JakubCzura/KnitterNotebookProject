namespace KnitterNotebook.Models
{
    public class Yarn
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Project Project = new();
    }
}