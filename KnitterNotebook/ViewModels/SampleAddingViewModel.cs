﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
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
    public partial class SampleAddingViewModel : BaseViewModel
    {
        public SampleAddingViewModel(ISampleService sampleService, IUserService userService, IValidator<CreateSampleDto> createSampleDtoValidator)
        {
            _sampleService = sampleService;
            _userService = userService;
            _createSampleDtoValidator = createSampleDtoValidator;
            DeletePhotoCommand = new RelayCommand(() => SourceImagePath = null);
        }

        private readonly ISampleService _sampleService;

        private readonly IUserService _userService;

        private readonly IValidator<CreateSampleDto> _createSampleDtoValidator;

        public ICommand DeletePhotoCommand { get; }

        [ObservableProperty]
        private string _yarnName = string.Empty;

        [ObservableProperty]
        private int _loopsQuantity;

        [ObservableProperty]
        private int _rowsQuantity;

        [ObservableProperty]
        private double _needleSize;

        [ObservableProperty]
        public NeedleSizeUnit _needleSizeUnit = NeedleSizeUnit.mm;

        [ObservableProperty]
        private string _description = string.Empty;

        public static IEnumerable<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>();

        [ObservableProperty]
        private string? _sourceImagePath = null;

        public static Action NewSampleAdded { get; set; } = null!;

        public static void OnNewSampleAdded() => NewSampleAdded.Invoke();

        [RelayCommand]
        private void ChooseImage()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp"
            };
            dialog.ShowDialog();
            SourceImagePath = dialog.FileName;
        }

        [RelayCommand]
        private async Task AddSampleAsync()
        {
            try
            {
                CreateSampleDto createSampleDto = new(YarnName, LoopsQuantity, RowsQuantity, NeedleSize, NeedleSizeUnit, Description, LoggedUserInformation.Id, SourceImagePath);
                ValidationResult validation = await _createSampleDtoValidator.ValidateAsync(createSampleDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas dodawania próbki obliczeniowej");
                    return;
                }
                await _sampleService.CreateAsync(createSampleDto);
                OnNewSampleAdded();
                MessageBox.Show("Zapisano nową próbkę obliczeniową");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}