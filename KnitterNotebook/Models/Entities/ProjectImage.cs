using System;

namespace KnitterNotebook.Models.Entities;

public class ProjectImage : BaseImage
{
    public ProjectImage()
    {
    }

    public ProjectImage(string path, int projectId)
    {
        Path = path;
        ProjectId = projectId;
    }

    public DateTime DateTime { get; } = DateTime.UtcNow;

    public virtual Project Project { get; set; } = default!;

    public int ProjectId { get; set; }
}