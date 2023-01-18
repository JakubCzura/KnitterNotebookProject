using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for MovieUrlAddingWindow.xaml
    /// </summary>
    public class MovieUrlAddingWindowViewModel : BaseViewModel
    {

        public ICommand AddMovieUrlCommandAsync { get; private set; }

        private MovieUrl movieUrl = null!;

        public MovieUrlAddingWindowViewModel()
        {
            AddMovieUrlCommandAsync = new AsyncRelayCommand(AddMovieUrlAsync);
        }

        private async Task AddMovieUrlAsync()
        {
            throw new NotImplementedException("Implement AddMovieUrlAsync in MovieUrlAddingsWindowViewModel");
        }

        public MovieUrl MovieUrl
        {
            get { return movieUrl; }
            set { movieUrl = value; OnPropertyChanged(); }
        }
    }
}
