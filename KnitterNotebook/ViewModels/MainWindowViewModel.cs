using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Views.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
        }

        public ICommand ShowSettingsWindowCommand { get; set; }

        private void ShowSettingsWindow()
        {
            SettingsWindow SettingsWindow = new();
            SettingsWindow.ShowDialog();
        }
    }
}