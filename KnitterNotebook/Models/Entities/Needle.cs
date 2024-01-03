using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Needle(double size, NeedleSizeUnit sizeUnit) : BaseDbEntity
{
    public double Size { get; set; } = size;

    public NeedleSizeUnit SizeUnit { get; set; } = sizeUnit;

    public virtual Project Project { get; set; } = default!;
}