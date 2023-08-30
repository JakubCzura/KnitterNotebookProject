using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for MovieUrlAddingWindow.xaml
    /// </summary>
    public partial class MovieUrlAddingViewModel : BaseViewModel
    {
        public MovieUrlAddingViewModel(ILogger<MovieUrlAddingViewModel> logger, IMovieUrlService movieUrlService, IValidator<CreateMovieUrlDto> createMovieUrlValidator)
        {
            _logger = logger;
            _movieUrlService = movieUrlService;
            _createMovieUrlValidator = createMovieUrlValidator;
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        public static void OnNewMovieUrlAdded() => NewMovieUrlAdded?.Invoke();


        #endregion Delegates

        #region Properties

        private readonly ILogger<MovieUrlAddingViewModel> _logger;
        private readonly IMovieUrlService _movieUrlService;
        private readonly IValidator<CreateMovieUrlDto> _createMovieUrlValidator;

        [ObservableProperty]
        private string _link = string.Empty;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string? _description;

        #endregion Properties

        #region Methods

        [RelayCommand]
        private async Task AddMovieUrlAsync()
        {
            try
            {
                CreateMovieUrlDto createMovieUrl = new(Title, Link, Description, LoggedUserInformation.Id);
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
                _logger.LogError(exception, "Error while adding new movie's url");
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}