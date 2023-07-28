namespace KnitterNotebook.Models.Dtos
{
    public record CreateSampleDto(string YarnName, int LoopsQuantity, int RowsQuantity, double NeedleSize, string NeedleSizeUnit, string? Description, int UserId, string? SourceImagePath);
}