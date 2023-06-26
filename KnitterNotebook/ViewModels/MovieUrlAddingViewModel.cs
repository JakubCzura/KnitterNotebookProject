using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Security.RightsManagement;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for MovieUrlAddingWindow.xaml
    /// </summary>
    public class MovieUrlAddingViewModel : BaseViewModel
    {
        public MovieUrlAddingViewModel(DatabaseContext databaseContext, IMovieUrlService movieUrlService, IValidator<CreateMovieUrl> createMovieUrlValidator, IUserService userService)
        {
            _databaseContext = databaseContext;
            _movieUrlService = movieUrlService;
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
            _createMovieUrlValidator = createMovieUrlValidator;
            _userService = userService;
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        public static void OnNewMovieUrlAdded()
        {
            NewMovieUrlAdded?.Invoke();
        }

        #endregion Delegates

        #region Properties

        private readonly DatabaseContext _databaseContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly IUserService _userService;
        private readonly IValidator<CreateMovieUrl> _createMovieUrlValidator;
        private string _link = string.Empty;
        private string _title = string.Empty;
        public ICommand AddMovieUrlCommandAsync { get; }

        public string Link
        {
            get { return _link; }
            set { _link = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private async Task AddMovieUrlAsync()
        {
            try
            {
                User? user = await _userService.GetAsync(LoggedUserInformation.Id);
                if (user == null)
                {
                    MessageBox.Show("Błąd podczas dodania filmu", "Nie odnaleziono użytkownika");
                    return;
                }
                CreateMovieUrl createMovieUrl = new(Title, Link, user);
                var validation = _createMovieUrlValidator.Validate(createMovieUrl);
                if (validation.IsValid)
                {
                    await _movieUrlService.CreateAsync(createMovieUrl);
                    OnNewMovieUrlAdded();
                    MessageBox.Show("Dodano nowy film");
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