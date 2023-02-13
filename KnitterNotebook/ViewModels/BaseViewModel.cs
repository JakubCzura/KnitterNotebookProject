using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

        #region Methods

        public void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected UserControl windowContent;

        public UserControl WindowContent
        {
            get { return windowContent; }
            protected set { windowContent = value; OnPropertyChanged(); }
        }
        #endregion Methods
    }
}