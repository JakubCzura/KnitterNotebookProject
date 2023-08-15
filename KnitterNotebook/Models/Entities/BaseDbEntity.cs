using System.ComponentModel.DataAnnotations;

namespace KnitterNotebook.Models.Entities
{
    public abstract class BaseDbEntity
    {
        [Key]
        public int Id { get; set; }
    }
}