using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KnitterNotebook.Helpers.Extensions.DependencyRegistration;

public static class ServicesRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
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

        return services;
    }
}