using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Needle(double size, NeedleSizeUnit sizeUnit) : BaseDbEntity
{
    public double Size { get; set; } = size;

    public NeedleSizeUnit SizeUnit { get; set; } = sizeUnit;

    public Project Project { get; set; } = new();
}