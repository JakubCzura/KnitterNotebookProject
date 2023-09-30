using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// Base class for all view models
/// </summary>
public abstract class BaseViewModel : ObservableObject
{
    public static void ShowWindow<T>() where T : Window => App.AppHost?.Services.GetService<T>()?.Show();

    public static void Closewindow(Window instance) => instance?.Close();
}