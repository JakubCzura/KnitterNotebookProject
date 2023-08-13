using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public record CreateSampleDto(string YarnName, int LoopsQuantity, int RowsQuantity, double NeedleSize, NeedleSizeUnit NeedleSizeUnit, string? Description, int UserId, string? SourceImagePath);
}