namespace KnitterNotebook.Models.Entities;

public class PatternPdf(string path) : BaseDbEntity
{
    public string Path { get; set; } = path;

    public virtual Project Project { get; set; } = default!;

    public int ProjectId { get; set; }
}