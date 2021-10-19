namespace FeatureApp.Dependancies
{
    using FeatureApp.Common.Models;
    using FeatureApp.Core;
    using FeatureApp.Infrastructure.DataAccess;
    using FeatureApp.Infrastructure.Service;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add Feature App to Web Host.
        /// </summary>
        /// <param name="services">Service collections to be injected.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>Added Feature Application to Service Application.</returns>
        public static IServiceCollection AddFeatureApp(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddFeatureDatabase(configuration)
                .AddFeatureService();

            return services;
        }

        /// <summary>
        /// Creates feature service.
        /// </summary>
        /// <returns>Added Cosmos Feature Service to Service collection.</returns>
        private static IServiceCollection AddFeatureService(this IServiceCollection services)
        {
            services.AddSingleton<IFeatureService, FeatureService>();

            return services;
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key.
        /// </summary>
        /// <returns>Added Cosmos Feature Database to Service collection.</returns>
        private static IServiceCollection AddFeatureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRepository<Feature>>(CreateCosmosDbService(configuration));

            return services;
        }

        private static CosmosRepository<Feature> CreateCosmosDbService(IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("CosmosDb");
            var databaseName = configurationSection.GetSection("DatabaseName").Value;
            var containerName = configurationSection.GetSection("ContainerName").Value;
            var account = configurationSection.GetSection("Account").Value;
            var key = configurationSection.GetSection("Key").Value;
            var partitionKeyPath = configurationSection.GetSection("PartitionKeyPath").Value;

            var client = new CosmosClient(account, key);
            var cosmosRepository = new CosmosRepository<Feature>(client, databaseName, containerName);
            var database = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();

            database.Database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath).GetAwaiter().GetResult();

            return cosmosRepository;
        }
    }
}
