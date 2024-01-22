using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace KnitterNotebook.Helpers.Extensions.DependencyRegistration;

public static class WindowsRegistration
{
    public static IServiceCollection RegisterWindows(this IServiceCollection services)
    {
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

        return services;
    }
}