namespace KnitterNotebook.Models.Entities;

public class PatternPdf : BaseDbEntity
{
    public PatternPdf(string path)
    {
        Path = path;
    }

    public string Path { get; set; } = string.Empty;

    public virtual Project Project { get; set; } = new();

    public int ProjectId { get; set; }
}