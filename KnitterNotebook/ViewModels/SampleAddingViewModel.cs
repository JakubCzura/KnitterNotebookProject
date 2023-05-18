using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SampleAddingViewModel : BaseViewModel
    {
        public SampleAddingViewModel(ISampleService sampleService)
        {
            _sampleService = sampleService;
            ShowDialogWindowCommand = new RelayCommand(ShowDialogWindow);
            SaveFileCommand = new RelayCommand(SaveFile);
            DeletePhotoCommand = new RelayCommand(DeletePhoto);
            AddSampleCommandAsync = new AsyncRelayCommand(AddSampleAsync);
        }

        private readonly ISampleService _sampleService;

        public ICommand ShowDialogWindowCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand DeletePhotoCommand { get; }

        public ICommand AddSampleCommandAsync { get; }

        private string _yarnName = string.Empty;

        public string YarnName
        {
            get { return _yarnName; }
            set { _yarnName = value; OnPropertyChanged(); }
        }

        private int _loopsQuantity;

        public int LoopsQuantity
        {
            get { return _loopsQuantity; }
            set { _loopsQuantity = value; OnPropertyChanged(); }
        }

        private int _rowsQuantity;

        public int RowsQuantity
        {
            get { return _rowsQuantity; }
            set { _rowsQuantity = value; OnPropertyChanged(); }
        }

        private double _needleSize;

        public double NeedleSize
        {
            get { return _needleSize; }
            set { _needleSize = value; OnPropertyChanged(); }
        }

        public string _needleSizeUnit = string.Empty;

        public string NeedleSizeUnit
        {
            get { return _needleSizeUnit; }
            set { _needleSizeUnit = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public static IEnumerable<string> NeedleSizeUnits => new[] { "mm", "cm" };

        private string fileName = string.Empty;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(); }
        }

        public bool CreateUserDirectory(string userDirectoryName)
        {
            string path = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "UsersDirectories");
            path = Path.Combine(path, userDirectoryName);

            File.Create(path);
            return File.Exists(path);
        }

        public string GetPhotoName(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public string GetUserDirectoryName(string userDirectoryName)
        {
            return Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "UsersDirectories", userDirectoryName);
        }

        private void DeletePhoto()
        {
            FileName = string.Empty;
        }

        private void SaveFile()
        {
            var dir = (Path.Combine(GetUserDirectoryName("Test")));
            Directory.CreateDirectory(dir);
            var newPhoto = Path.Combine(dir, Path.GetFileName(FileName));
            if (File.Exists(newPhoto))
            {
                MessageBox.Show("File exists");
            }
            else
            {
                File.Copy(FileName, newPhoto);
            }
        }

        private void ShowDialogWindow()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ShowDialog();
            FileName = dialog.FileName;
            //Path.Combine(GetUserDirectoryName("Test"), GetPhotoName(dialog.FileName));
            //Directory.CreateDirectory(Path.Combine(GetUserDirectoryName("Test")));
            //File.Create(FileName);
        }

        private async Task AddSampleAsync()
        {
            var ImagePath = string.Empty;
            CreateSampleDto createSampleDto = new(YarnName, LoopsQuantity, RowsQuantity, NeedleSize, NeedleSizeUnit, Description, LoggedUserInformation.Id, ImagePath);
            await _sampleService.CreateAsync(createSampleDto);
        }
    }
}