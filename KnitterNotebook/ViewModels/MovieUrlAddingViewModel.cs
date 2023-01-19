using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Interfaces;
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
        public MovieUrlAddingViewModel()
        {
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
        }

        #region Delegates

        public static Action NewMovieUrlAdded { get; set; } = null!;

        #endregion Delegates

        #region Properties

        public ICommand AddMovieUrlCommandAsync { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        private IAddingMovieUrl AddingMovieUrl { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        private string link;

        public string Link
        {
            get { return link; }
            set { link = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private async Task AddMovieUrlAsync()
        {
            try
            {
                using (KnitterNotebookContext = new KnitterNotebookContext())
                {
                    User user = LoggedUserInformation.LoggedUser;
                    Theme theme = await KnitterNotebookContext.Themes.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUser.ThemeId);
                    user.Theme = theme;
                    KnitterNotebookContext.Attach(user);
                    KnitterNotebookContext.Attach(theme);
                    MovieUrl movieUrl = new() { Title = Title, Link = new Uri(Link), User = user, UserId = user.Id };
                    IValidator<MovieUrl> movieUrlValidator = new MovieUrlValidator();
                    AddingMovieUrl = new AddingMovieUrl();
                    if (movieUrlValidator.Validate(movieUrl))
                    {
                        await AddingMovieUrl.AddMovieUrl(movieUrl, KnitterNotebookContext);
                        NewMovieUrlAdded?.Invoke();
                        MessageBox.Show("Dodano nowy film");
                    }
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