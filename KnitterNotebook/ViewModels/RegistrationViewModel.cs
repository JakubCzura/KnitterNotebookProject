using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Registration;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.Windows;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel()
        {
            RegisterUserCommandAsync = new AsyncRelayCommand(RegisterUser);
        }

        #region Properties

        private User User { get; set; } 

        private Theme Theme { get; set; }
        private IRegistration StandardRegistration { get; set; }
        private RegistrationManager RegistrationManager { get; set; }

        public ICommand RegisterUserCommandAsync { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

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

        #endregion Properties

        #region Methods

        private async Task RegisterUser()
        {
            try
            {
                using (KnitterNotebookContext = new KnitterNotebookContext())
                {
                    Theme = KnitterNotebookContext.Themes.First();
                    KnitterNotebookContext.Attach(Theme);
                    User = new() { Nickname = Nickname, Email = Email, Password = RegistrationWindow.Instance.UserPasswordPasswordBox.Password, Theme = Theme };
                    IValidator<User> userValidator = new UserValidator();
                    if (userValidator.Validate(User))
                    {
                        User.Password = PasswordHasher.HashPassword(User.Password);
                        StandardRegistration = new StandardRegistration();
                        RegistrationManager = new(StandardRegistration, User, KnitterNotebookContext);
                        await RegistrationManager.Register();
                        Window.GetWindow(RegistrationWindow.Instance).Close();
                        MessageBox.Show("Rejestracja przebiegła pomyślnie");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}