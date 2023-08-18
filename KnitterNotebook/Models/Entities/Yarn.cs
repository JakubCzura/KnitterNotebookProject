namespace KnitterNotebook.Models.Entities
{
    public class Yarn : BaseDbEntity
    {
        public Yarn(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = string.Empty;

        public Project Project { get; set; } = new();
    }
}