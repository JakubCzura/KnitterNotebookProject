using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models
{
    public class NullableSizeNeedle
    {
        private double? _size;

        public double? Size
        {
            get => _size;
            set
            {
                if (value <= 0)
                {
                    _size = null;
                }
                else
                {
                    _size = value;
                }
            }
        }

        public NeedleSizeUnit SizeUnit { get; set; } = NeedleSizeUnit.mm;
    }
}