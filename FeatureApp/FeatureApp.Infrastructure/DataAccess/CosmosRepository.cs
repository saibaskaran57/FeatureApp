namespace FeatureApp.Infrastructure.DataAccess
{
    using System.Net;
    using System.Threading.Tasks;
    using FeatureApp.Common;
    using FeatureApp.Core;
    using Microsoft.Azure.Cosmos;

    public sealed class CosmosRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="client">The cosmos client.</param>
        /// <param name="databaseName">The cosmos database name.</param>
        /// <param name="containerName">The cosmos container name.</param>
        public CosmosRepository(CosmosClient client, string databaseName, string containerName)
        {
            Guard.RequiresNotNull(client, nameof(client));
            Guard.RequiresNotEmpty(databaseName, nameof(databaseName));
            Guard.RequiresNotEmpty(containerName, nameof(containerName));

            this.container = client.GetContainer(databaseName, containerName);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(string key)
        {
            Guard.RequiresNotEmpty(key, nameof(key));

            try
            {
                var response = await this.container.ReadItemAsync<TEntity>(key, new PartitionKey(key));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task SaveAsync(TEntity entity)
        {
            Guard.RequiresNotNull(entity, nameof(entity));

            await this.container.CreateItemAsync(entity);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(string key, TEntity entity)
        {
            Guard.RequiresNotEmpty(key, nameof(key));
            Guard.RequiresNotNull(entity, nameof(entity));

            await this.container.ReplaceItemAsync(entity, key);
        }
    }
}
