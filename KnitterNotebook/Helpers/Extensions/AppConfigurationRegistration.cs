using KnitterNotebook.ApplicationInformation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace KnitterNotebook.Helpers.Extensions;

public static class AppConfigurationRegistration
{
    public static IHostBuilder RegisterAppConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
        {
            configurationBuilder.AddJsonFile(Paths.AppSettings, false, true);
        });

        return hostBuilder;
    }
}