using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Sample : BaseDbEntity
{
    public string YarnName { get; set; } = string.Empty;

    public int LoopsQuantity { get; set; }

    public int RowsQuantity { get; set; }

    public double NeedleSize { get; set; }

    public NeedleSizeUnit NeedleSizeUnit { get; set; }

    public string? Description { get; set; } = null;

    public virtual SampleImage? Image { get; set; } = null;

    public int UserId { get; set; }
}