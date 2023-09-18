using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public class NeedleDto
{
    public NeedleDto(Needle needle)
    {
        Id = needle.Id;
        Size = needle.Size;
        SizeUnit = needle.SizeUnit;
    }

    public int Id { get; set; }

    public double Size { get; set; }

    public NeedleSizeUnit SizeUnit { get; set; }
}