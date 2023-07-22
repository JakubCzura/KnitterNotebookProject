using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
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

    public partial class ProjectPlanningViewModel : ObservableObject
    {
        public ProjectPlanningViewModel(IProjectService projectService, IValidator<PlanProjectDto> planProjectDtoValidator)
        {
            _projectService = projectService;
            DeletePatternPdfCommand = new RelayCommand(() => PatternPdfPath = null);
            _planProjectDtoValidator = planProjectDtoValidator;
        }

        private readonly IProjectService _projectService;
        private readonly IValidator<PlanProjectDto> _planProjectDtoValidator;

        public ICommand DeletePatternPdfCommand { get; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate = null;

        [ObservableProperty]
        private string _yarnsWithSemicolons = string.Empty;

        [ObservableProperty]
        private string _patternName = string.Empty;

        //Tzw. "inne"
        [ObservableProperty]
        private string? _description = null;

        [ObservableProperty]
        private string? _patternPdfPath = null;

        [ObservableProperty]
        private NullableSizeNeedle _needle1 = new(null, NeedleSizeUnits.Units.mm.ToString());

        [ObservableProperty]
        private NullableSizeNeedle _needle2 = new(null, NeedleSizeUnits.Units.mm.ToString());

        [ObservableProperty]
        private NullableSizeNeedle _needle3 = new(null, NeedleSizeUnits.Units.mm.ToString());

        [ObservableProperty]
        private NullableSizeNeedle _needle4 = new(null, NeedleSizeUnits.Units.mm.ToString());

        [ObservableProperty]
        private NullableSizeNeedle _needle5 = new(null, NeedleSizeUnits.Units.mm.ToString());

        public static IEnumerable<string> NeedleSizeUnitList => NeedleSizeUnits.UnitsList;

        [RelayCommand]
        private void ChoosePatternPdf()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files (*.pdf)|*.pdf"
            };
            dialog.ShowDialog();
            PatternPdfPath = dialog.FileName;
        }

        public static Action NewProjectPlanned { get; set; } = null!;

        private static void OnNewProjectPlanned() => NewProjectPlanned?.Invoke();

        [RelayCommand]
        private async Task AddProjectAsync()
        {
            IEnumerable<NullableSizeNeedle> needles = NullableSizeNeedlesFilter.GetNeedlesWithSizeHasValue(Needle1, Needle2, Needle3, Needle4, Needle5);
            IEnumerable<CreateNeedleDto> needlesToCreate = needles.Select(x => new CreateNeedleDto(Convert.ToDouble(x.Size), x.SizeUnit));
            IEnumerable<CreateYarnDto> yarnsNames = !string.IsNullOrWhiteSpace(YarnsWithSemicolons) ? YarnsWithSemicolons.Split(',').Select(x => new CreateYarnDto(x)) : Enumerable.Empty<CreateYarnDto>();

            try
            {
                PlanProjectDto planProjectDto = new(Name, StartDate, PatternName, needlesToCreate, yarnsNames, Description, PatternPdfPath, LoggedUserInformation.Id);
                ValidationResult validation = await _planProjectDtoValidator.ValidateAsync(planProjectDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas planowania projektu");
                    return;
                }
                await _projectService.PlanProjectAsync(planProjectDto);
                OnNewProjectPlanned();
                MessageBox.Show("Zaplanowano nowy projekt");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}