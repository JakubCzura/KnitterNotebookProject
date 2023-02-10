using KnitterNotebook.Views.UserControls;
using System.Windows.Controls;

namespace KnitterNotebook.ViewModels.Helpers
{
    public class MainWindowContent
    {
        /// <summary>
        /// Chooses content of MainWindow - projects or other user control that user wants to see
        /// </summary>
        /// <param name="userControlName">Name of user control, for example ProjectsUserControl</param>
        /// <returns>New instance of chosen user control</returns>
        public static UserControl ChooseMainWindowContent(string userControlName)
        {
            return userControlName switch
            {
                nameof(PlannedProjectsUserControl) => new PlannedProjectsUserControl(),
                nameof(ProjectsInProgressUserControl) => new ProjectsInProgressUserControl(),
                nameof(ProjectsUserControl) => new ProjectsUserControl(),
                nameof(SamplesUserControl) => new SamplesUserControl(),
                _ => new ProjectsUserControl()
            };
        }
    }
}