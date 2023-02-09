using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace KnitterNotebook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      
        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddSingleton<LoginWindow>();
                     services.AddSingleton<LoginViewModel>();

                     //services.AddDbContext<KnitterNotebookContext>(
                     //    options =>
                     //    {
                     //        string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
                     //        string appSettingsString = File.ReadAllText(appSettingsPath);
                     //        AppSettings AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString)!;                          
                     //        options.UseSqlServer(AppSettings.KnitterNotebookConnectionString);
                     //    });
                 })
                 .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupWindow = AppHost.Services.GetRequiredService<LoginWindow>();
            startupWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }     
    }
}