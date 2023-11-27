namespace KnitterNotebook.Models.Entities;

public class Yarn(string name) : BaseDbEntity
{
    public string Name { get; set; } = name;

    public Project Project { get; set; } = new();
}