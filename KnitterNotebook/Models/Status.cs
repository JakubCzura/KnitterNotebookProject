﻿using System.Collections.Generic;
using System.Windows.Documents;

namespace KnitterNotebook.Models
{
    public class Status
    {
        public int Id { get; set; }

        public string ProjectStatus { get; set; } = string.Empty;

        public virtual List<Project> Projects { get; set; } = new();
    }
}