using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Registration;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class RegistrationWindowViewModel : BaseViewModel
    {
        private string nickname;

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        RegistrationManager RegistrationManager { get; set; }

        public ICommand RegisterUserCommandAsync { get; private set; }

        KnitterNotebookContext KnitterNotebookContext { get; set; }
        public RegistrationWindowViewModel()
        {
            RegisterUserCommandAsync = new AsyncRelayCommand(RegisterUser);
        }

        //AppSettings AppSettings { get; set; }

        private async Task RegisterUser()
        {
            try
            {
                //string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
                // string appSettingsString = File.ReadAllText(appSettingsPath);
                //  AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString);
                // var contextOptions = new DbContextOptionsBuilder<KnitterNotebookContext>().UseSqlServer(AppSettings.KnitterNotebookConnectionString).Options;
                using (KnitterNotebookContext = new KnitterNotebookContext())
                {
                    Theme theme = KnitterNotebookContext.Themes.First();
                    KnitterNotebookContext.Attach(theme);
                    User user = new() { Nickname = Nickname, Email = Email, Password = RegistrationWindow.Instance.UserPasswordPasswordBox.Password, Theme = theme };
                    IValidator<User> userValidator = new UserValidator();
                    if (userValidator.Validate(user))
                    {
                        user.Password = PasswordHasher.HashPassword(user.Password);
                        StandardRegistration standardRegistration = new();
                        RegistrationManager = new(standardRegistration, user, KnitterNotebookContext);
                        if (await RegistrationManager.Register())
                        {
                            Window.GetWindow(RegistrationWindow.Instance).Close();
                            MessageBox.Show("Rejestracja przebiegła pomyślnie");
                        }
                        else
                        {
                            MessageBox.Show("Błąd w trakcie rejestracji");
                        }
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
