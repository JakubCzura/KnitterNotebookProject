using System.ComponentModel.DataAnnotations;

namespace KnitterNotebook.Models.Entities;

/// <summary>
/// Base entity to provide Id as primary key for entities
/// </summary>
public abstract class BaseDbEntity
{
    /// <summary>
    /// Id as primary key of entity
    /// </summary>
    [Key]
    public int Id { get; set; }
}