using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectImageAddingViewModel : BaseViewModel
    {
        public ProjectImageAddingViewModel(IProjectImageService projectImageService, IValidator<CreateProjectImageDto> createProjectImageDtoValidator, SharedResourceViewModel sharedResourceViewModel)
        {
            _projectImageService = projectImageService;
            _createProjectImageDtoValidator = createProjectImageDtoValidator;
            _sharedResourceViewModel = sharedResourceViewModel;
        }

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
            if (_sharedResourceViewModel.SelectedProjectInProgressId.HasValue)
            {
                CreateProjectImageDto addProjectImageDto = new(_sharedResourceViewModel.SelectedProjectInProgressId.Value, SourceImagePath, LoggedUserInformation.Id);
                ValidationResult validation = await _createProjectImageDtoValidator.ValidateAsync(addProjectImageDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas dodania zdjęcia projektu");
                    return;
                }
                await _projectImageService.CreateAsync(addProjectImageDto);
                _sharedResourceViewModel.OnProjectInProgressImageAdded();
                Closewindow(ProjectImageAddingWindow.Instance);
            }
        }
    }
}