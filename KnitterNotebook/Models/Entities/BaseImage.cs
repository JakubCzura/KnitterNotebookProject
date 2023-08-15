namespace KnitterNotebook.Models.Entities
{
    public abstract class BaseImage : BaseDbEntity
    {
        public string Path { get; set; } = string.Empty;
    }
}