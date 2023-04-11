using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
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
        public MovieUrlAddingViewModel(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        #endregion Delegates

        #region Properties

        private readonly KnitterNotebookContext _knitterNotebookContext;
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
                User user = await _knitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                //KnitterNotebookContext.Attach(user);
                MovieUrl movieUrl = new()
                {
                    Title = Title,
                    Link = new Uri(Link),
                    User = user
                };
                IValidator<MovieUrl> movieUrlValidator = new MovieUrlValidator();
                AddingMovieUrl addingMovieUrl = new();
                if (movieUrlValidator.Validate(movieUrl))
                {
                    await addingMovieUrl.AddMovieUrl(movieUrl, _knitterNotebookContext);
                    NewMovieUrlAdded?.Invoke();
                    MessageBox.Show("Dodano nowy film");
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