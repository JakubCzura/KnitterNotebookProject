namespace KnitterNotebook.Models.Entities;

public class Yarn(string name) : BaseDbEntity
{
    public string Name { get; set; } = name;

    public virtual Project Project { get; set; } = default!;
}