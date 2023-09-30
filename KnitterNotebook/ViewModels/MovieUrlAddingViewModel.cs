using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for MovieUrlAddingWindow.xaml
/// </summary>
public partial class MovieUrlAddingViewModel : BaseViewModel
{
    public MovieUrlAddingViewModel(ILogger<MovieUrlAddingViewModel> logger,
        IMovieUrlService movieUrlService,
        IValidator<CreateMovieUrlDto> createMovieUrlValidator,
        SharedResourceViewModel sharedResourceViewModel)
    {
        _logger = logger;
        _movieUrlService = movieUrlService;
        _createMovieUrlValidator = createMovieUrlValidator;
        _sharedResourceViewModel = sharedResourceViewModel;
    }

    private readonly ILogger<MovieUrlAddingViewModel> _logger;
    private readonly IMovieUrlService _movieUrlService;
    private readonly IValidator<CreateMovieUrlDto> _createMovieUrlValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    #region Events

    public static Action NewMovieUrlAdded { get; set; } = null!;

    public static void OnNewMovieUrlAdded() => NewMovieUrlAdded?.Invoke();

    #endregion Events

    #region Properties

    [ObservableProperty]
    private string _link = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _description;

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async Task AddMovieUrlAsync()
    {
        try
        {
            CreateMovieUrlDto createMovieUrl = new(Title, Link, Description, _sharedResourceViewModel.UserId);
            ValidationResult validation = await _createMovieUrlValidator.ValidateAsync(createMovieUrl);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage);
                return;
            }
            await _movieUrlService.CreateAsync(createMovieUrl);
            OnNewMovieUrlAdded();
            MessageBox.Show(Translations.NewMovieUrlAdded);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding movie's url");
            MessageBox.Show(Translations.ErrorWhileAddingMovieUrl);
        }
    }

    #endregion Commands
}