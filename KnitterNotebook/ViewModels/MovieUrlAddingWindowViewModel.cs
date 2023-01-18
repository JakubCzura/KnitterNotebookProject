using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database.Registration;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KnitterNotebook.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using static OneOf.Types.TrueFalseOrNull;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for MovieUrlAddingWindow.xaml
    /// </summary>
    public class MovieUrlAddingWindowViewModel : BaseViewModel
    {

        public ICommand AddMovieUrlCommandAsync { get; private set; }

        public MovieUrlAddingWindowViewModel()
        {
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
        }

        public static Action AddingNewMovieUrl { get; set; } = null!;


        KnitterNotebookContext KnitterNotebookContext { get; set; }

        IAddingMovieUrl AddingMovieUrl { get; set; }

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
                        AddingNewMovieUrl?.Invoke();
                        MessageBox.Show("Dodano nowy film");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

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
    }
}
