using KnitterNotebook.ViewModels;
using KnitterNotebook.Views.UserControls;
using Microsoft.Extensions.DependencyInjection;

namespace KnitterNotebook.Helpers.Extensions;

public static class UserControlsRegistration
{
    public static IServiceCollection RegisterUserControls(this IServiceCollection services)
    {
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

        return services;
    }
}