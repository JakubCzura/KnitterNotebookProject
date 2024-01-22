using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;

namespace KnitterNotebook;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App() => AppHost = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
        {
            configurationBuilder.AddJsonFile(Paths.AppSettings, false, true);
        })
        .RegisterSerilog()
        .ConfigureServices((hostContext, services) =>
        {
            services.RegisterDatabase(hostContext);

            services.RegisterValidators();

            services.RegisterServices();

            services.RegisterViewModels();

            services.RegisterWindows();

            services.RegisterUserControls();
        })
        .Build();

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        LoginWindow startupWindow = AppHost.Services.GetService<LoginWindow>()!;
        startupWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}