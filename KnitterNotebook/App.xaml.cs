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
using Serilog;
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
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DatabaseContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("KnitterNotebookConnectionString"));
                    });

                    services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
                    services.AddScoped<IValidator<ChangeNicknameDto>, ChangeNicknameDtoValidator>();
                    services.AddScoped<IValidator<ChangeEmailDto>, ChangeEmailDtoValidator>();
                    services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
                    services.AddScoped<IValidator<ChangeThemeDto>, ChangeThemeDtoValidator>();
                    services.AddScoped<IValidator<CreateMovieUrlDto>, CreateMovieUrlDtoValidator>();
                    services.AddScoped<IValidator<CreateSampleDto>, CreateSampleDtoValidator>();
                    services.AddScoped<IValidator<CreateYarnDto>, CreateYarnDtoValidator>();
                    services.AddScoped<IValidator<CreateNeedleDto>, CreateNeedleDtoValidator>();
                    services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
                    services.AddScoped<IValidator<PlanProjectDto>, PlanProjectDtoValidator>();
                    services.AddScoped<IValidator<LogInDto>, LogInDtoValidator>();
                    services.AddScoped<IValidator<CreateProjectImageDto>, CreateProjectImageDtoValidator>();
                    services.AddScoped<IMovieUrlService, MovieUrlService>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IThemeService, ThemeService>();
                    services.AddScoped<ISampleService, SampleService>();
                    services.AddScoped<IProjectService, ProjectService>();
                    services.AddScoped<IPasswordService, PasswordService>();
                    services.AddScoped<IWindowContentService, WindowContentService>();
                    services.AddScoped<IWebBrowserService, WebBrowserService>();
                    services.AddScoped<IProjectImageService, ProjectImageService>();
                    services.AddScoped<IEmailService, EmailService>();
                    services.AddScoped<ITokenService, TokenService>();
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
                    services.AddSingleton(s => new SharedResourceViewModel());
                    services.AddSingleton(s => new MainViewModel(s.GetRequiredService<IMovieUrlService>(),
                                                                 s.GetRequiredService<ISampleService>(),
                                                                 s.GetRequiredService<IUserService>(),
                                                                 s.GetRequiredService<IProjectService>(),
                                                                 s.GetRequiredService<IWindowContentService>(),
                                                                 s.GetRequiredService<IThemeService>(),
                                                                 s.GetRequiredService<IWebBrowserService>(),
                                                                 s.GetRequiredService<IProjectImageService>(),
                                                                 s.GetRequiredService<SharedResourceViewModel>()));
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
                    services.AddTransient(x => new PlannedProjectsUserControl()
                    {
                        DataContext = x.GetRequiredService<MainViewModel>()
                    });
                    services.AddTransient(x => new ProjectsInProgressUserControl()
                    {
                        DataContext = x.GetRequiredService<MainViewModel>()
                    });
                    services.AddTransient(x => new FinishedProjectsUserControl()
                    {
                        DataContext = x.GetRequiredService<MainViewModel>()
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
                    services.AddTransient<ProjectPlanningViewModel>();
                    services.AddTransient(s => new ProjectPlanningWindow()
                    {
                        DataContext = s.GetRequiredService<ProjectPlanningViewModel>()
                    });
                    services.AddTransient<PdfBrowserViewModel>();
                    services.AddTransient(s => new PdfBrowserWindow()
                    {
                        DataContext = s.GetRequiredService<PdfBrowserViewModel>()
                    });
                    services.AddTransient<ProjectImageAddingViewModel>();
                    services.AddTransient(s => new ProjectImageAddingWindow()
                    {
                        DataContext = s.GetRequiredService<ProjectImageAddingViewModel>()
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