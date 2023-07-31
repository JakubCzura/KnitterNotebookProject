using KnitterNotebook.Models.Enums;
using System.Windows.Controls;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IWindowContentService
    {
        public UserControl ChooseMainWindowContent(MainWindowContent userControlName);

        public UserControl ChooseSettingsWindowContent(SettingsWindowContent userControlName);
    }
}
