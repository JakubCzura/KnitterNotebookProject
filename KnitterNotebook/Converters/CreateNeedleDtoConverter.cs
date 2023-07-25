using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Converters
{
    public class CreateNeedleDtoConverter
    {
        public static CreateNeedleDto Convert(NullableSizeNeedle nullableSizeNeedle)
            => new(System.Convert.ToDouble(nullableSizeNeedle.Size), nullableSizeNeedle.SizeUnit);
    }
}
