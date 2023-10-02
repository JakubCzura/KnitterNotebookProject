﻿using System;

namespace KnitterNotebook.Models.Entities;

public class ProjectImage : BaseImage
{
    public DateTime DateTime { get; } = DateTime.Now;

    public Project Project { get; set; } = default!;

    public int ProjectId { get; set; }
}