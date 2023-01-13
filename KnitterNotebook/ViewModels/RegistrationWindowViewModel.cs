using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Registration;
using KnitterNotebook.Models;
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

        public ICommand RegisterUserCommand { get; private set; }

        KnitterNotebookContext KnitterNotebookContext { get; set; }
        public RegistrationWindowViewModel()
        {
            RegisterUserCommand = new AsyncRelayCommand(RegisterUser);
        }
        
        AppSettings AppSettings { get; set; }

        private async Task<bool> RegisterUser()
        {
            User user = new() { Nickname = Nickname, Email = Email, Password = RegistrationWindow.Instance.UserPasswordPasswordBox.Password };
            StandardRegistration standardRegistration = new();

            string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
            string appSettingsString = File.ReadAllText(appSettingsPath);
            AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString);

            var contextOptions = new DbContextOptionsBuilder<KnitterNotebookContext>().UseSqlServer(AppSettings.KnitterNotebookConnectionString).Options;
            KnitterNotebookContext = new(contextOptions);
            RegistrationManager = new(standardRegistration, user, KnitterNotebookContext);
            return await RegistrationManager.Register();
        }
    }
}
