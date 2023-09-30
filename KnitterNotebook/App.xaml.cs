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

namespace KnitterNotebook;

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
                configurationBuilder.AddJsonFile("appsettings.json", false, true);
            })
            .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("KnitterNotebookConnectionString"),
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                });

                #region Validators

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
                services.AddScoped<IValidator<ChangeProjectStatusDto>, ChangeProjectStatusDtoValidator>();
                services.AddScoped<IValidator<EditProjectDto>, EditProjectDtoValidator>();

                #endregion Validators

                #region Services

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

                #endregion Services

                #region ViewModels

                services.AddSingleton<SharedResourceViewModel>();
                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegistrationViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MovieUrlAddingViewModel>();
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SampleAddingViewModel>();
                services.AddTransient<ResetPasswordViewModel>();
                services.AddTransient<ProjectPlanningViewModel>();
                services.AddTransient<PdfBrowserViewModel>();
                services.AddTransient<ProjectImageAddingViewModel>();
                services.AddTransient<ProjectEditingViewModel>();

                #endregion ViewModels

                #region Windows

                services.AddTransient(s => new LoginWindow()
                {
                    DataContext = s.GetRequiredService<LoginViewModel>()
                });
                services.AddTransient(s => new RegistrationWindow()
                {
                    DataContext = s.GetService<RegistrationViewModel>()
                });
                services.AddTransient(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
                services.AddTransient(s => new MovieUrlAddingWindow()
                {
                    DataContext = s.GetRequiredService<MovieUrlAddingViewModel>()
                });
                services.AddTransient(s => new SettingsWindow()
                {
                    DataContext = s.GetRequiredService<SettingsViewModel>()
                });
                services.AddTransient(s => new SampleAddingWindow()
                {
                    DataContext = s.GetRequiredService<SampleAddingViewModel>()
                });
                services.AddTransient(s => new ResetPasswordWindow()
                {
                    DataContext = s.GetRequiredService<ResetPasswordViewModel>()
                });
                services.AddTransient(s => new ProjectPlanningWindow()
                {
                    DataContext = s.GetRequiredService<ProjectPlanningViewModel>()
                });
                services.AddTransient(s => new PdfBrowserWindow()
                {
                    DataContext = s.GetRequiredService<PdfBrowserViewModel>()
                });
                services.AddTransient(s => new ProjectImageAddingWindow()
                {
                    DataContext = s.GetRequiredService<ProjectImageAddingViewModel>()
                });
                services.AddTransient(s => new ProjectEditingWindow()
                {
                    DataContext = s.GetRequiredService<ProjectEditingViewModel>()
                });

                #endregion Windows

                #region UserControls

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

                #endregion UserControls
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