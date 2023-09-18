using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System;

namespace KnitterNotebook.Helpers.Extensions;

public static class ProjectExtension
{
    /// <summary>
    /// Adjusts <paramref name="project"/>'s properties when <paramref name="project"/>'s status is changed
    /// </summary>
    /// <param name="project">Project whose status is changed</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void AdjustProjectWhenChangingStatus(this Project project, ProjectStatusName projectNewStatusName)
    {
        if (project.ProjectStatus == ProjectStatusName.Planned && projectNewStatusName == ProjectStatusName.InProgress)
        {
            project.StartDate = DateTime.Today;
        }
        else if (project.ProjectStatus == ProjectStatusName.InProgress && projectNewStatusName == ProjectStatusName.Planned)
        {
            project.StartDate = null;
        }

        if (project.ProjectStatus == ProjectStatusName.InProgress && projectNewStatusName == ProjectStatusName.Finished)
        {
            project.EndDate = DateTime.Today;
        }
        else if (project.ProjectStatus == ProjectStatusName.Finished && projectNewStatusName == ProjectStatusName.InProgress)
        {
            project.EndDate = null;
        }

        project.ProjectStatus = projectNewStatusName;
    }
}