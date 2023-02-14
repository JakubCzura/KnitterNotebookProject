using CommunityToolkit.Mvvm.Input;
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
            ChooseSettingsWindowContentCommand = new RelayCommand<Type>(ChooseSettingsWindowContent!);
        }

        public ICommand ChooseSettingsWindowContentCommand { get; private set; }

        private void ChooseSettingsWindowContent(Type userControl)
        {
            try
            {
                WindowContent = (Activator.CreateInstance(userControl) as UserControl)!;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna ustawień");
                WindowContent = new UserSettingsUserControl();
            }
        }
    }
}