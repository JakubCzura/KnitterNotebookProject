using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for SampleAddingWindow.xaml
/// </summary>
public partial class SampleAddingViewModel(ILogger<SampleAddingViewModel> logger,
    ISampleService sampleService,
    IValidator<CreateSampleDto> createSampleDtoValidator,
    SharedResourceViewModel sharedResourceViewModel) : BaseViewModel
{
    private readonly ILogger<SampleAddingViewModel> _logger = logger;
    private readonly ISampleService _sampleService = sampleService;
    private readonly IValidator<CreateSampleDto> _createSampleDtoValidator = createSampleDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel = sharedResourceViewModel;

    #region Events

    public static Action NewSampleAdded { get; set; } = null!;

    public static void OnNewSampleAdded() => NewSampleAdded.Invoke();

    #endregion Events

    #region Properties

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
    private void DeletePhoto() => SourceImagePath = null;

    [RelayCommand]
    private async Task AddSampleAsync()
    {
        try
        {
            CreateSampleDto createSampleDto = new(YarnName, LoopsQuantity, RowsQuantity, NeedleSize, NeedleSizeUnit, Description, _sharedResourceViewModel.UserId, SourceImagePath);
            ValidationResult validation = await _createSampleDtoValidator.ValidateAsync(createSampleDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage);
                return;
            }
            await _sampleService.CreateAsync(createSampleDto);
            OnNewSampleAdded();
            MessageBox.Show(Translations.NewSampleAdded);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding new sample");
            MessageBox.Show(Translations.ErrorWhileAddingNewSample);
        }
    }

    #endregion Commands
}