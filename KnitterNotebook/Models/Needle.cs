using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models
{
    public class Needle
    {
        public Needle()
        {
        }

        public Needle(double size, NeedleSizeUnit sizeUnit)
        {
            Size = size;
            SizeUnit = sizeUnit;
        }

        public int Id { get; set; }

        public double Size { get; set; }

        public NeedleSizeUnit SizeUnit { get; set; }

        public Project Project { get; set; } = new();
    }
}