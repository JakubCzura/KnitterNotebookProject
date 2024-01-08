using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Sample : BaseDbEntity
{
    public Sample()
    {
    }

    public Sample(CreateSampleDto createSampleDto, SampleImage? sampleImage)
    {
        YarnName = createSampleDto.YarnName;
        LoopsQuantity = createSampleDto.LoopsQuantity;
        RowsQuantity = createSampleDto.RowsQuantity;
        NeedleSize = createSampleDto.NeedleSize;
        NeedleSizeUnit = createSampleDto.NeedleSizeUnit;
        Description = createSampleDto.Description;
        UserId = createSampleDto.UserId;
        Image = sampleImage;
    }

    public string YarnName { get; set; } = string.Empty;

    public int LoopsQuantity { get; set; }

    public int RowsQuantity { get; set; }

    public double NeedleSize { get; set; }

    public NeedleSizeUnit NeedleSizeUnit { get; set; }

    public string? Description { get; set; } = null;

    public virtual SampleImage? Image { get; set; } = default;

    public int UserId { get; set; }
}