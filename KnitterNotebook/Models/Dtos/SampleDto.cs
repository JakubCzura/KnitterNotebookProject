using KnitterNotebook.Models.Entities;

namespace KnitterNotebook.Models.Dtos;

public class SampleDto : BasicSampleDto
{
    public SampleDto(Sample sample) : base(sample)
    {
        Id = sample.Id;
        YarnName = sample.YarnName;
        LoopsQuantity = sample.LoopsQuantity;
        RowsQuantity = sample.RowsQuantity;
        NeedleSize = sample.NeedleSize;
        NeedleSizeUnit = sample.NeedleSizeUnit;
        Description = sample.Description;
        ImagePath = sample.Image?.Path;
    }

    public string? Description { get; set; }

    //If sample doesn't have related image the path is null
    public string? ImagePath { get; set; }
}