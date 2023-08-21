using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectImageAddingViewModel : BaseViewModel
    {
        public ProjectImageAddingViewModel(IProjectImageService projectImageService, SharedResourceViewModel sharedResourceViewModel)
        {
            _projectImageService = projectImageService;
            _sharedResourceViewModel = sharedResourceViewModel;
        }

        private readonly IProjectImageService _projectImageService;
        private readonly SharedResourceViewModel _sharedResourceViewModel;

        [ObservableProperty]
        private string _sourceImagePath = string.Empty;

        [RelayCommand]
        private void ChooseImage()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png"
            };
            dialog.ShowDialog();
            SourceImagePath = dialog.FileName;
        }

        [RelayCommand]
        private async Task AddProjectImageAsync()
        {
            if (_sharedResourceViewModel.SelectedProjectInProgressId.HasValue)
            {
                CreateProjectImageDto addProjectImageDto = new(_sharedResourceViewModel.SelectedProjectInProgressId.Value, SourceImagePath);
                await _projectImageService.CreateAsync(addProjectImageDto);
                _sharedResourceViewModel.OnProjectInProgressImageAdded();
                Closewindow(ProjectImageAddingWindow.Instance);
            }
        }
    }
}