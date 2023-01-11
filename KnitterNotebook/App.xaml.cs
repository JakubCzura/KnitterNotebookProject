using KnitterNotebook.Database;
using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        public App()
        {
            host = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                // ConfigureServices(context.Configuration, services);
                //services.AddDbContext<KnitterNotebookContext>(options =>
                //{
                //    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=KnitterNotebookDb;Trusted_Connection=True;");
                //});


                IConfiguration configuration;

                configuration = new ConfigurationBuilder()
                    .AddJsonFile(@"appsettings.json")
                    .Build();
                services.AddDbContext<KnitterNotebookContext>(options =>
                                    options.UseSqlServer(configuration.GetConnectionString("KnitterNotebookConnectionString")));

                services.AddScoped<LoginWindowViewModel>();

                services.AddSingleton<LoginWindowViewModel>();
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
