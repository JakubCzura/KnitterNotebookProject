using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectImageAddingViewModel : BaseViewModel
    {
        public ProjectImageAddingViewModel()
        {
        }

        [ObservableProperty]
        private string? _sourceImagePath = null;

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
            throw new NotImplementedException("ProjectImageAddingViewModel - AddProjectImageAsync");
        }
    }
}