using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class BaseViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Methods

        public static void ShowWindow<T>() where T : Window
            => App.AppHost?.Services.GetService<T>()?.Show();

        public static void Closewindow(Window instance)
           => Window.GetWindow(instance)?.Close();

        public static void LogOut() => Environment.Exit(0);

        #endregion Methods
    }
}