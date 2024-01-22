using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KnitterNotebook.Helpers.Extensions;

public static class DatabaseRegistration
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(hostBuilderContext.Configuration.GetConnectionString(DatabaseContext.DatabaseConnectionStringKey),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });

        return services;
    }
}