using System;
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

        protected object windowContent;
       
        /// <summary>
        /// Object that can be set as ContentControl.Content 
        /// </summary>
        public object WindowContent
        {
            get { return windowContent; }
            set { windowContent = value; OnPropertyChanged(); }
        }
        #endregion Methods
    }
}