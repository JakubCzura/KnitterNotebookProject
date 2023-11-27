using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public class NeedleDto(Needle needle)
{
    public int Id { get; set; } = needle.Id;

    public double Size { get; set; } = needle.Size;

    public NeedleSizeUnit SizeUnit { get; set; } = needle.SizeUnit;
}