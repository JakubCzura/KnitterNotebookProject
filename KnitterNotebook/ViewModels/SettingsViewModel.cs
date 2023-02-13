using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ViewModels.Helpers;
using KnitterNotebook.Views.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            WindowContent = new UserSettingsUserControl();
            ChooseSettingsWindowContentCommand = new RelayCommand<string>(ChooseSettingsWindowContent!);
        }

        public ICommand ChooseSettingsWindowContentCommand { get; private set; }

        private void ChooseSettingsWindowContent(string userControlName)
        {
            try
            {
                WindowContent = SettingsWindowContent.ChooseSettingsWindowContent(userControlName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna ustawień");
                WindowContent = new UserSettingsUserControl();
            }
        }
    }
}