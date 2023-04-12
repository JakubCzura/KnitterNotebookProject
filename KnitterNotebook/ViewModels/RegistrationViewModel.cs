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
        public RegistrationViewModel(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
            RegisterUserCommandAsync = new AsyncRelayCommand(RegisterUser);
        }

        #region Properties

        private readonly KnitterNotebookContext _knitterNotebookContext;
        private string _email = string.Empty;
        private string _nickname = string.Empty;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; OnPropertyChanged(); }
        }

        public ICommand RegisterUserCommandAsync { get; }

        #endregion Properties

        #region Methods

        private async Task RegisterUser()
        {
            try
            {
                Theme theme = _knitterNotebookContext.Themes.First();
                User user = new()
                {
                    Nickname = Nickname,
                    Email = Email,
                    Password = RegistrationWindow.Instance.UserPasswordPasswordBox.Password,
                    Theme = theme
                };
                if (await UserExistence.IfUserAlreadyExists(user, _knitterNotebookContext) == false)
                {
                    IValidator<User> userValidator = new UserValidator();
                    if (userValidator.Validate(user))
                    {
                        user.Password = PasswordHasher.HashPassword(user.Password);
                        IRegistration standardRegistration = new StandardRegistration();
                        RegistrationManager registrationManager = new(standardRegistration, user, _knitterNotebookContext);
                        await registrationManager.Register();
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