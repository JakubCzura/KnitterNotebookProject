using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models;

public class NullableSizeNeedle
{
    private double? _size;

    public double? Size
    {
        get => _size;
        set => _size = value <= 0 ? null : value;
    }

    public NeedleSizeUnit SizeUnit { get; set; } = NeedleSizeUnit.mm;
}