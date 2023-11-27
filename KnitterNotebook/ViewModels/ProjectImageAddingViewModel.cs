using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for ProjectImageAddingWindow.xaml
/// </summary>
public partial class ProjectImageAddingViewModel(ILogger<ProjectImageAddingViewModel> logger,
    IProjectImageService projectImageService,
    IValidator<CreateProjectImageDto> createProjectImageDtoValidator,
    SharedResourceViewModel sharedResourceViewModel) : BaseViewModel
{
    private readonly ILogger<ProjectImageAddingViewModel> _logger = logger;
    private readonly IProjectImageService _projectImageService = projectImageService;
    private readonly IValidator<CreateProjectImageDto> _createProjectImageDtoValidator = createProjectImageDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel = sharedResourceViewModel;

    #region Properties

    [ObservableProperty]
    private string _sourceImagePath = string.Empty;

    #endregion Properties

    #region Commands

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
                    MessageBox.Show(errorMessage);
                    return;
                }
                await _projectImageService.CreateAsync(addProjectImageDto);
                _sharedResourceViewModel.OnProjectInProgressImageAdded();
                Closewindow(ProjectImageAddingWindow.Instance);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding project's photo");
            MessageBox.Show(Translations.ErrorWhileAddingProjectPhoto);
        }
    }

    #endregion Commands
}