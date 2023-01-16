using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}