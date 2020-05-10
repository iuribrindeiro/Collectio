using System;
using Collectio.Infra.CrossCutting.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Collectio.Presentation.Infra
{
    public static class DatabaseMigrator
    {
        public static IHost Migrate(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbMigrator = services.GetRequiredService<IDatabaseMigrator>();
                    dbMigrator.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return host;
        }
    }
}