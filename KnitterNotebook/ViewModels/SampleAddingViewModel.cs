using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    internal class SampleAddingViewModel : BaseViewModel
    {
        public SampleAddingViewModel()
        {
            ShowDialogWindowCommand = new RelayCommand(ShowDialogWindow);
            SaveFileCommand = new RelayCommand(SaveFile);
        }

        public ICommand ShowDialogWindowCommand { get; }
        public ICommand SaveFileCommand { get; }

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

        private int _needleSize;

        public int NeedleSize
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
    }
}