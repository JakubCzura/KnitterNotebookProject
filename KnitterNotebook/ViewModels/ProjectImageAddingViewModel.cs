using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

public partial class ProjectImageAddingViewModel : BaseViewModel
{
    public ProjectImageAddingViewModel(ILogger<ProjectImageAddingViewModel> logger, IProjectImageService projectImageService, IValidator<CreateProjectImageDto> createProjectImageDtoValidator, SharedResourceViewModel sharedResourceViewModel)
    {
        _logger = logger;
        _projectImageService = projectImageService;
        _createProjectImageDtoValidator = createProjectImageDtoValidator;
        _sharedResourceViewModel = sharedResourceViewModel;
    }

    private readonly ILogger<ProjectImageAddingViewModel> _logger;
    private readonly IProjectImageService _projectImageService;
    private readonly IValidator<CreateProjectImageDto> _createProjectImageDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    [ObservableProperty]
    private string _sourceImagePath = string.Empty;

    [RelayCommand]
    private void ChooseImage()
    {
        OpenFileDialog dialog = new()
        {
            Filter = FileDialogFilter.ImageFilter
        };
        dialog.ShowDialog();
        SourceImagePath = dialog.FileName;
    }

    [RelayCommand]
    private async Task AddProjectImageAsync()
    {
        try
        {
            if (_sharedResourceViewModel.SelectedProjectInProgressId.HasValue)
            {
                CreateProjectImageDto addProjectImageDto = new(_sharedResourceViewModel.SelectedProjectInProgressId.Value, SourceImagePath, _sharedResourceViewModel.UserId);
                ValidationResult validation = await _createProjectImageDtoValidator.ValidateAsync(addProjectImageDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas dodania zdjęcia projektu");
                    return;
                }
                await _projectImageService.CreateAsync(addProjectImageDto);
                _sharedResourceViewModel.OnProjectInProgressImageAdded();
                Closewindow(ProjectImageAddingWindow.Instance);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding project's image");
            MessageBox.Show("Błąd podczas dodawania zdjęcia projektu");
        }
    }
}