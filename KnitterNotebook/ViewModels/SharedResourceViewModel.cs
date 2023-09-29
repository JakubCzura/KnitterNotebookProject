using KnitterNotebook.Models.Enums;
using System;
using System.Collections.Generic;

namespace KnitterNotebook.ViewModels;

public partial class SharedResourceViewModel : BaseViewModel
{
    /// <summary>
    /// Id of user that has successfully logged in
    /// </summary>
    public int UserId { get; set; }

    public Action<int> UserUpdatedInDatabase { get; set; } = null!;

    public void OnUserUpdatedInDatabase() => UserUpdatedInDatabase?.Invoke(UserId);

    //Nullable as selected project in progress can be null
    public int? SelectedProjectInProgressId { get; set; }

    public Action<int> ProjectInProgressImageAdded { get; set; } = null!;

    public void OnProjectInProgressImageAdded()
    {
        if (SelectedProjectInProgressId.HasValue)
        {
            ProjectInProgressImageAdded?.Invoke(SelectedProjectInProgressId.Value);
        }
    }

    public string? PatternPdfPath { get; set; }

    public List<string> FilesToDelete { get; set; } = new();

    public (int, ProjectStatusName) EditedProjectIdAndStatus { get; set; }

    public Action<int, ProjectStatusName> ProjectEdited { get; set; } = null!;

    public void OnProjectEdited() => ProjectEdited?.Invoke(EditedProjectIdAndStatus.Item1, EditedProjectIdAndStatus.Item2);
}