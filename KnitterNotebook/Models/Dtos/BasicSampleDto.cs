using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public class BasicSampleDto
    {
        public BasicSampleDto(Sample sample)
        {
            Id = sample.Id;
            YarnName = sample.YarnName;
            LoopsQuantity = sample.LoopsQuantity;
            RowsQuantity = sample.RowsQuantity;
            NeedleSize = sample.NeedleSize;
            NeedleSizeUnit = sample.NeedleSizeUnit;
        }

        public int Id { get; set; }

        public string YarnName { get; set; }

        public int LoopsQuantity { get; set; }

        public int RowsQuantity { get; set; }

        public double NeedleSize { get; set; }

        public NeedleSizeUnit NeedleSizeUnit { get; set; }
    }
}