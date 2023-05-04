using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for MovieUrlAddingWindow.xaml
    /// </summary>
    public class MovieUrlAddingViewModel : BaseViewModel
    {
        public MovieUrlAddingViewModel(KnitterNotebookContext knitterNotebookContext, IMovieUrlService movieUrlService, IValidator<CreateMovieUrl> createMovieUrlValidator)
        {
            _knitterNotebookContext = knitterNotebookContext;
            _movieUrlService = movieUrlService;
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
            _createMovieUrlValidator = createMovieUrlValidator;
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        #endregion Delegates

        #region Properties

        private readonly KnitterNotebookContext _knitterNotebookContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly IValidator<CreateMovieUrl> _createMovieUrlValidator;
        private string _link = string.Empty;
        private string _title = string.Empty;
        public ICommand AddMovieUrlCommandAsync { get; }

        public string Link
        {
            get { return _link; }
            set { _link = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private async Task AddMovieUrlAsync()
        {
            try
            {
                User? user = await _knitterNotebookContext.Users.FindAsync(LoggedUserInformation.Id);
                if(user != null) 
                {
                    CreateMovieUrl createMovieUrl = new(Title, Link);
                    var validation = _createMovieUrlValidator.Validate(createMovieUrl);
                    if (validation.IsValid)
                    {
                        await _movieUrlService.CreateAsync(createMovieUrl);
                        NewMovieUrlAdded?.Invoke();
                        MessageBox.Show("Dodano nowy film");
                    }
                }  
                else
                {
                    MessageBox.Show("Błąd podczas dodania filmu", "Nie odnaleziono użytkownika");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}