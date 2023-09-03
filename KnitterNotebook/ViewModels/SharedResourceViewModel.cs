using System;

namespace KnitterNotebook.ViewModels
{
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
    }
}