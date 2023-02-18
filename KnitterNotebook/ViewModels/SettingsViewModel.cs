using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            SetUserSettingsUserControlVisibleCommand = new RelayCommand(() => 
            { 
                SetUserControlsVisibilityHidden(); UserSettingsUserControlVisibility = Visibility.Visible; 
            });
            SetThemeSettingsUserControlVisibleCommand = new RelayCommand(() => 
            {
                SetUserControlsVisibilityHidden(); ThemeSettingsUserControlVisibility = Visibility.Visible;
            });
            ChangeNicknameCommandAsync = new AsyncRelayCommand(ChangeNicknameAsync);
        }

        // private string newNickname = string.Empty;
        private string newNickname;

        public string NewNickname
        {
            get { return newNickname; }
            set { newNickname = value; OnPropertyChanged(); }
        }

        private Visibility userSettingsUserControlVisibility = Visibility.Visible;

        public Visibility UserSettingsUserControlVisibility
        {
            get { return userSettingsUserControlVisibility; }
            set { userSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private Visibility themeSettingsUserControlVisibility = Visibility.Hidden;

        public Visibility ThemeSettingsUserControlVisibility
        {
            get { return themeSettingsUserControlVisibility; }
            set { themeSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        public ICommand SetUserSettingsUserControlVisibleCommand { get; private set; }

        public ICommand SetThemeSettingsUserControlVisibleCommand { get; private set; }

        public ICommand ChangeNicknameCommandAsync { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        private void SetUserControlsVisibilityHidden()
        {
            try
            {
                UserSettingsUserControlVisibility = Visibility.Hidden;
                ThemeSettingsUserControlVisibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna ustawień");
            }
        }

        private async Task ChangeNicknameAsync()
        {
            try
            {
                using (KnitterNotebookContext = new())
                {
                    User? user = await KnitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                    if (user == null)
                    {
                        MessageBox.Show("Błąd zmiany nicku");
                    }
                    else
                    {
                        user.Nickname = NewNickname;
                        KnitterNotebookContext.Users.Update(user);
                        await KnitterNotebookContext.SaveChangesAsync();
                        MessageBox.Show(NewNickname);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}