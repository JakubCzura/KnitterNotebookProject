using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        private AppSettings appSettings;
        public App()
        {
           // appSettings = new();
            host = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                // ConfigureServices(context.Configuration, services);
                //services.AddDbContext<KnitterNotebookContext>(options =>
                //{
                //    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=KnitterNotebookDb;Trusted_Connection=True;");
                //});

                //MessageBox.Show("project " + ProjectDirectory.ProjectDirectoryFullPath);
                //MessageBox.Show("solution " + SolutionDirectory.SolutionDirectoryFullPath);

                string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
                string appSettingsString = File.ReadAllText(appSettingsPath);
                appSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString);

                //IConfiguration configuration;

                //configuration = new ConfigurationBuilder()
                //    //.AddJsonFile(@"appsettings.json")
                //    .AddJsonFile(appSettingsPath)
                //    .Build();
                services.AddDbContext<KnitterNotebookContext>(options =>
                                     options.UseSqlServer(appSettings.KnitterNotebookConnectionString));
                                    // options.UseSqlServer(configuration
                                    // .GetConnectionString("KnitterNotebookConnectionString")));
                                    // options.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = KnitterNotebookDb; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"));
            

                // services.AddScoped<LoginWindowViewModel>();

                // services.AddSingleton<LoginWindowViewModel>();
            })
            .Build();

            //using(var serviceScope = host.Services.CreateScope())
            //{
            //    var services = serviceScope.ServiceProvider;
            //    try
            //    {
            //        var masterWindow = services.GetRequiredService<LoginWindow>();
            //        masterWindow.Show();
            //    }
            //    catch(Exception exception) 
            //    {
            //        MessageBox.Show(exception.Message);
            //    }
            //}
        }

        //private void ConfigureServices(IConfiguration configuration,
        //IServiceCollection services)
        //{
        //    // ...
        //    services.AddSingleton<LoginWindow>();

        //    services.Configure<AppSettings>
        //    (configuration.GetSection(nameof(AppSettings)));

        //    services.AddScoped<ISampleService, SampleService>();

        //    //...
        //}

        //protected override async void OnStartup(StartupEventArgs e)
        //{
        //    await host.StartAsync();

        //    var LoginWindow = host.Services.GetRequiredService<LoginWindow>();
        //    LoginWindow.Show();

        //    base.OnStartup(e);
        //}

        //protected override async void OnExit(ExitEventArgs e)
        //{
        //    using (host)
        //    {
        //        await host.StopAsync(TimeSpan.FromSeconds(5));
        //    }

        //    base.OnExit(e);
        //}
    }
}
