using Microsoft.Extensions.Hosting;
using Serilog;

namespace KnitterNotebook.Helpers.Extensions.DependencyRegistration;

public static class SerilogRegistration
{
    public static IHostBuilder RegisterSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((hostContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
        });

        return hostBuilder;
    }
}