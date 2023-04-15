using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels.Services.Interfaces;
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
        public MovieUrlAddingViewModel(KnitterNotebookContext knitterNotebookContext, IMovieUrlService movieUrlService)
        {
            _knitterNotebookContext = knitterNotebookContext;
            _movieUrlService = movieUrlService;
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        #endregion Delegates

        #region Properties

        private readonly KnitterNotebookContext _knitterNotebookContext;
        private readonly IMovieUrlService _movieUrlService;
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
                User? user = await _knitterNotebookContext.Users.FindAsync(LoggedUserInformation.LoggedUserId);
                if(user != null) 
                {
                    MovieUrl movieUrl = new(Title, new Uri(Link), user);
                    IModelsValidator<MovieUrl> movieUrlValidator = new MovieUrlValidator();
                    if (movieUrlValidator.Validate(movieUrl))
                    {
                        await _movieUrlService.AddMovieUrlAsync(movieUrl);
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