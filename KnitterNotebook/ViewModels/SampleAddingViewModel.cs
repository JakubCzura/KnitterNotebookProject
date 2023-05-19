using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Win32;
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
            ChooseImageCommand = new RelayCommand(ChooseImage);
            DeletePhotoCommand = new RelayCommand(() => FileName = null);
            AddSampleCommandAsync = new AsyncRelayCommand(AddSampleAsync);
        }

        private readonly ISampleService _sampleService;
        public ICommand ChooseImageCommand { get; }
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

        public string _needleSizeUnit = "mm";

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

        private string? fileName = null;

        public string? FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(); }
        }

        private static void SaveFile(string fileToSave, string newFile)
        {
            new FileInfo(newFile)?.Directory?.Create();
            if (File.Exists(newFile))
            {
                MessageBox.Show("Plik o podanej nazwie już istnieje, podaj inny plik lub zmień jego nazwę przed wyborem");
            }
            else
            {
                File.Copy(fileToSave, newFile);
            }
        }

        private void ChooseImage()
        {
            OpenFileDialog dialog = new();
            dialog.ShowDialog();
            FileName = dialog.FileName;
        }

        private async Task AddSampleAsync()
        {
            string? imagePath = null;
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                imagePath = Path.Combine(Paths.UserDirectory("Test"), Path.GetFileName(FileName));
                SaveFile(FileName, imagePath);
            }
            CreateSampleDto createSampleDto = new(YarnName, LoopsQuantity, RowsQuantity, NeedleSize, NeedleSizeUnit, Description, LoggedUserInformation.Id, imagePath);
            await _sampleService.CreateAsync(createSampleDto);
            MessageBox.Show("Zapisano nową próbkę obliczeniową");
        }
    }
}