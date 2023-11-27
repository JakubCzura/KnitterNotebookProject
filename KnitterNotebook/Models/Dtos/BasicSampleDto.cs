using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public class BasicSampleDto(Sample sample)
{
    public int Id { get; set; } = sample.Id;

    public string YarnName { get; set; } = sample.YarnName;

    public int LoopsQuantity { get; set; } = sample.LoopsQuantity;

    public int RowsQuantity { get; set; } = sample.RowsQuantity;

    public double NeedleSize { get; set; } = sample.NeedleSize;

    public NeedleSizeUnit NeedleSizeUnit { get; set; } = sample.NeedleSizeUnit;
}