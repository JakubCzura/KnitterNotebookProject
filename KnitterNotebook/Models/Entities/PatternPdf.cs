namespace KnitterNotebook.Models.Entities;

public class PatternPdf(string path) : BaseDbEntity
{
    public string Path { get; set; } = path;

    public virtual Project Project { get; set; } = new();

    public int ProjectId { get; set; }
}