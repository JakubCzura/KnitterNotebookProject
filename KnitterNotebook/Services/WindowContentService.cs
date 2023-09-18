using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.UserControls;
using System.Windows.Controls;

namespace KnitterNotebook.Services;

public class WindowContentService : IWindowContentService
{
    public UserControl ChooseMainWindowContent(MainWindowContent userControlName) => userControlName switch
    {
        MainWindowContent.PlannedProjectsUserControl => new PlannedProjectsUserControl(),
        MainWindowContent.ProjectsInProgressUserControl => new ProjectsInProgressUserControl(),
        MainWindowContent.ProjectsUserControl => new FinishedProjectsUserControl(),
        MainWindowContent.SamplesUserControl => new SamplesUserControl(),
        _ => new SamplesUserControl()
    };

    public UserControl ChooseSettingsWindowContent(SettingsWindowContent userControlName) => userControlName switch
    {
        SettingsWindowContent.UserSettingsUserControl => new UserSettingsUserControl(),
        SettingsWindowContent.ThemeSettingsUserControl => new ThemeSettingsUserControl(),
        _ => new UserSettingsUserControl()
    };
}