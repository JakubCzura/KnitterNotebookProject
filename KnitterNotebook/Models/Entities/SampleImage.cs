﻿namespace KnitterNotebook.Models.Entities;

public class SampleImage : BaseImage
{
    public SampleImage(string path)
    {
        Path = path;
    }

    public virtual Sample Sample { get; set; } = new();

    public int SampleId { get; set; }
}