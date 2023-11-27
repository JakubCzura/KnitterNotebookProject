using KnitterNotebook.Models.Enums;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Entities;

public class Theme : BaseDbEntity
{
    public ApplicationTheme Name { get; set; }

    public virtual List<User> Users { get; set; } = [];
}