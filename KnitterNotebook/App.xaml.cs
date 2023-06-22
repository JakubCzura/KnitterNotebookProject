using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private IConfiguration Configuration { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DatabaseContext>(options =>
                    {
                        IConfigurationBuilder builder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        Configuration = builder.Build();
                        //string appSettingsPath = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "appsettings.json");
                        //string appSettingsString = File.ReadAllText(appSettingsPath);
                        //AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsString)!;
                        options.UseSqlServer(Configuration.GetConnectionString("KnitterNotebookConnectionString"));
                    }, ServiceLifetime.Scoped);

                    services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
                    services.AddScoped<IValidator<ChangeNicknameDto>, ChangeNicknameDtoValidator>();
                    services.AddScoped<IValidator<ChangeEmailDto>, ChangeEmailDtoValidator>();
                    services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
                    services.AddScoped<IValidator<ChangeThemeDto>, ChangeThemeDtoValidator>();
                    services.AddScoped<IValidator<CreateMovieUrl>, CreateMovieUrlValidator>();
                    services.AddScoped<IValidator<CreateSampleDto>, CreateSampleDtoValidator>();
                    services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
                    services.AddScoped<IMovieUrlService, MovieUrlService>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IThemeService, ThemeService>();
                    services.AddScoped<ISampleService, SampleService>();
                    services.AddScoped<IImageService, ImageService>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient(s => new LoginWindow()
                    {
                        DataContext = s.GetRequiredService<LoginViewModel>()
                    });
                    services.AddTransient<RegistrationViewModel>();
                    services.AddTransient(s => new RegistrationWindow()
                    {
                        DataContext = s.GetService<RegistrationViewModel>()
                    });
                    services.AddSingleton(s => new MainViewModel(s.GetRequiredService<DatabaseContext>(), s.GetRequiredService<IMovieUrlService>(), s.GetRequiredService<ISampleService>()));
                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                    services.AddTransient<MovieUrlAddingViewModel>();
                    services.AddTransient(s => new MovieUrlAddingWindow()
                    {
                        DataContext = s.GetRequiredService<MovieUrlAddingViewModel>()
                    });
                    services.AddTransient<SettingsViewModel>();
                    services.AddTransient(s => new SettingsWindow()
                    {
                        DataContext = s.GetRequiredService<SettingsViewModel>()
                    });
                    services.AddTransient(x => new SamplesUserControl()
                    {
                        DataContext = x.GetRequiredService<MainViewModel>()
                    });
                    services.AddTransient(x => new UserSettingsUserControl()
                    {
                        DataContext = x.GetRequiredService<SettingsViewModel>()
                    });
                    services.AddTransient(x => new ThemeSettingsUserControl()
                    {
                        DataContext = x.GetRequiredService<SettingsViewModel>()
                    });
                    services.AddTransient<SampleAddingViewModel>();
                    services.AddTransient(s => new SampleAddingWindow()
                    {
                        DataContext = s.GetRequiredService<SampleAddingViewModel>()
                    });
                    services.AddTransient<ResetPasswordViewModel>();
                    services.AddTransient(s => new ResetPasswordWindow()
                    {
                        DataContext = s.GetRequiredService<ResetPasswordViewModel>()
                    });
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupWindow = AppHost.Services.GetService<LoginWindow>();
            startupWindow?.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}