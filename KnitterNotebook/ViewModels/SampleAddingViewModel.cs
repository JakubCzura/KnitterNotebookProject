﻿using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SampleAddingViewModel : BaseViewModel
    {
        public SampleAddingViewModel(ISampleService sampleService, IUserService userService, IValidator<CreateSampleDto> createSampleDtoValidator)
        {
            _sampleService = sampleService;
            _userService = userService;
            _createSampleDtoValidator = createSampleDtoValidator;
            ChooseImageCommand = new RelayCommand(ChooseImage);
            DeletePhotoCommand = new RelayCommand(() => ImageName = null);
            AddSampleCommandAsync = new AsyncRelayCommand(AddSampleAsync);
        }

        private readonly ISampleService _sampleService;

        private readonly IUserService _userService;

        private readonly IValidator<CreateSampleDto> _createSampleDtoValidator;
        public ICommand ChooseImageCommand { get; }
        public ICommand DeletePhotoCommand { get; }
        public ICommand AddSampleCommandAsync { get; }

        private string _yarnName = string.Empty;

        public string YarnName
        {
            get => _yarnName;
            set { _yarnName = value; OnPropertyChanged(); }
        }

        private int _loopsQuantity;

        public int LoopsQuantity
        {
            get => _loopsQuantity;
            set { _loopsQuantity = value; OnPropertyChanged(); }
        }

        private int _rowsQuantity;

        public int RowsQuantity
        {
            get => _rowsQuantity;
            set { _rowsQuantity = value; OnPropertyChanged(); }
        }

        private double _needleSize;

        public double NeedleSize
        {
            get => _needleSize;
            set { _needleSize = value; OnPropertyChanged(); }
        }

        public string _needleSizeUnit = "mm";

        public string NeedleSizeUnit
        {
            get => _needleSizeUnit;
            set { _needleSizeUnit = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public static IEnumerable<string> NeedleSizeUnits => new[] { "mm", "cm" };

        private string? imageName = null;

        public string? ImageName
        {
            get => imageName;
            set { imageName = value; OnPropertyChanged(); }
        }

        public static Action NewSampleAdded { get; set; } = null!;

        public static void OnNewSampleAdded()
        {
            NewSampleAdded.Invoke();
        }

        private void ChooseImage()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp"
            };
            dialog.ShowDialog();
            ImageName = dialog.FileName;
        }

        private async Task AddSampleAsync()
        {
            try
            {
                User user = await _userService.GetAsync(LoggedUserInformation.Id);
                string? imagePath = ImagePathCreator.CreatePathToSaveImage(user.Nickname, ImageName);
                CreateSampleDto createSampleDto = new(YarnName, LoopsQuantity, RowsQuantity, NeedleSize, NeedleSizeUnit, Description, LoggedUserInformation.Id, imagePath);
                var validation = _createSampleDtoValidator.Validate(createSampleDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas dodawania próbki obliczeniowej", MessageBoxButton.OK);
                    return;
                }
                if (await _sampleService.CreateAsync(createSampleDto))
                {
                    if (!string.IsNullOrWhiteSpace(ImageName) && !string.IsNullOrWhiteSpace(imagePath))
                    {
                        FileHelper.CopyWithDirectoryCreation(ImageName, imagePath);
                    }
                    OnNewSampleAdded();
                }
                MessageBox.Show("Zapisano nową próbkę obliczeniową");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}