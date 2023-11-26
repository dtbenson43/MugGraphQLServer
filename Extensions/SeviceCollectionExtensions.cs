using MongoDB.Driver;
using Mug.Services.CosmosDb;

namespace Mug.Extensions
{
    public static class SeviceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDbService(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["AZURE_COSMOS_CONNECTIONSTRING"];
            var databaseName = config["AZURE_COSMOS_DATABASE"];

            if (connectionString == null) throw new ArgumentException("AZURE_COSMOS_CONNECTIONSTRING not configured.");
            if (databaseName == null) throw new ArgumentException("AZURE_COSMOS_DATABASE not configured.");

            services.AddSingleton(new CosmosDbService(connectionString, databaseName));

            return services;
        }
    }
}
