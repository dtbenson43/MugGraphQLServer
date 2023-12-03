using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mug.Services.AzureSqlDbIdentity;
using Mug.Services.CosmosDb;

namespace Mug.Extensions
{
    public static class SeviceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDbService(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["AZURE_COSMOS_CONNECTIONSTRING"];
            var databaseName = config["AZURE_COSMOS_DATABASE"];

            if (connectionString == null) throw new InvalidOperationException("AZURE_COSMOS_CONNECTIONSTRING not configured.");
            if (databaseName == null) throw new InvalidOperationException("AZURE_COSMOS_DATABASE not configured.");

            services.AddSingleton(new CosmosDbService(connectionString, databaseName));

            return services;
        }

        public static void AddAzureSqlDbIdentityService(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["AZURE_SQL_CONNECTIONSTRING"];

            if (connectionString == null) throw new InvalidOperationException("AZURE_SQL_CONNECTIONSTRING not configured.");

            // Add the DbContext using the connection string
            services.AddDbContext<AzureSqlDbIdentityService>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Identity services
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AzureSqlDbIdentityService>()
                .AddDefaultTokenProviders();

            // Configure Identity options (optional)
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings, lockout settings, user settings, etc.
            });
        }
    }
}