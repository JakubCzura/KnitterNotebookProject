using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Needle : BaseDbEntity
{
    public Needle(double size, NeedleSizeUnit sizeUnit)
    {
        Size = size;
        SizeUnit = sizeUnit;
    }

    public double Size { get; set; }

    public NeedleSizeUnit SizeUnit { get; set; }

    public Project Project { get; set; } = new();
}