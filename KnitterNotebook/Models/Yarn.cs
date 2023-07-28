namespace KnitterNotebook.Models
{
    public class Yarn
    {
        public Yarn()
        {
        }

        public Yarn(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Project Project { get; set; } = new();
    }
}