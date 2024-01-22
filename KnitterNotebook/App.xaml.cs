using FluentValidation;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers.Extensions;
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

    public App() => AppHost = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
        {
            configurationBuilder.AddJsonFile(Paths.AppSettings, false, true);
        })
        .UseSerilog((hostContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
        })
        .ConfigureServices((hostContext, services) =>
        {
            #region Database

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(hostContext.Configuration.GetConnectionString(DatabaseContext.DatabaseConnectionStringKey),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            #endregion Database

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

            services.RegisterWindows();

            #endregion Windows

            #region UserControls

            services.RegisterUserControls();

            #endregion UserControls
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