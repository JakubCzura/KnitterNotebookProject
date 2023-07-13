using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Linq;
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
        public MovieUrlAddingViewModel(IMovieUrlService movieUrlService, IValidator<CreateMovieUrlDto> createMovieUrlValidator)
        {
            _movieUrlService = movieUrlService;
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
            _createMovieUrlValidator = createMovieUrlValidator;
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        public static void OnNewMovieUrlAdded() => NewMovieUrlAdded?.Invoke();

        #endregion Delegates

        #region Properties

        private readonly IMovieUrlService _movieUrlService;
        private readonly IValidator<CreateMovieUrlDto> _createMovieUrlValidator;
        private string _link = string.Empty;
        private string _title = string.Empty;
        public ICommand AddMovieUrlCommandAsync { get; }

        public string Link
        {
            get => _link;
            set { _link = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private async Task AddMovieUrlAsync()
        {
            try
            {
                CreateMovieUrlDto createMovieUrl = new(Title, Link, LoggedUserInformation.Id);
                ValidationResult validation = await _createMovieUrlValidator.ValidateAsync(createMovieUrl);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas rejestracji");
                    return;
                }
                await _movieUrlService.CreateAsync(createMovieUrl);
                OnNewMovieUrlAdded();
                MessageBox.Show("Dodano nowy film");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}