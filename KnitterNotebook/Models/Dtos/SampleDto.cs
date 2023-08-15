using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public class SampleDto
    {
        public SampleDto(Sample sample)
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

        public int Id { get; set; }

        public string YarnName { get; set; }

        public int LoopsQuantity { get; set; }

        public int RowsQuantity { get; set; }

        public double NeedleSize { get; set; }

        public NeedleSizeUnit NeedleSizeUnit { get; set; }

        public string? Description { get; set; }

        //If sample doesn't have related image the path is null
        public string? ImagePath { get; set; }
    }
}