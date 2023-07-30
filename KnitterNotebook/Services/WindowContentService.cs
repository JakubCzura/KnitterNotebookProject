using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.UserControls;
using System.Windows.Controls;

namespace KnitterNotebook.Services
{
    public class WindowContentService : IWindowContentService
    {
        public UserControl ChooseMainWindowContent(MainWindowContentUserControls userControlName) => userControlName switch
        {
            MainWindowContentUserControls.PlannedProjectsUserControl => new PlannedProjectsUserControl(),
            MainWindowContentUserControls.ProjectsInProgressUserControl => new ProjectsInProgressUserControl(),
            MainWindowContentUserControls.ProjectsUserControl => new ProjectsUserControl(),
            MainWindowContentUserControls.SamplesUserControl => new SamplesUserControl(),
            _ => new SamplesUserControl()
        };
    }
}
