using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public record CreateNeedleDto(double Size, NeedleSizeUnit SizeUnit);
}