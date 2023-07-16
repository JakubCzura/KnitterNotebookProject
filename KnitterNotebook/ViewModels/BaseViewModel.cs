using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class BaseViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Events

        //public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

        #region Methods

        //public void OnPropertyChanged([CallerMemberName] string name = null!)
        //    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public static void ShowWindow<T>() where T : Window
            => App.AppHost?.Services.GetService<T>()?.Show();

        public static void Closewindow(Window instance)
           => Window.GetWindow(instance)?.Close();

        public static void LogOut() => Environment.Exit(0);

        #endregion Methods
    }
}