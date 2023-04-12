using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

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

        public static void ShowWindow<T>() where T : Window
        {
            T? window = App.AppHost?.Services.GetService<T>();
            window?.Show();
        }

        #endregion Methods
    }
}