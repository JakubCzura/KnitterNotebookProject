using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
