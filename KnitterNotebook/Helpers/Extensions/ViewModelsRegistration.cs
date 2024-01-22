using KnitterNotebook.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KnitterNotebook.Helpers.Extensions;

public static class ViewModelsRegistration
{
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
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

        return services;
    }
}