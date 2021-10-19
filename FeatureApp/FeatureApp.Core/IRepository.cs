namespace FeatureApp.Core
{
    using System.Threading.Tasks;

    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets entity from repository.
        /// </summary>
        /// <param name="key">The key to be searched.</param>
        /// <returns>Returns searched entity from repository.</returns>
        Task<TEntity> GetAsync(string key);

        /// <summary>
        /// Save entity in repository.
        /// </summary>
        /// <param name="entity">The entity to be saved.</param>
        /// <returns>Saves entity to repository.</returns>
        Task SaveAsync(TEntity entity);

        /// <summary>
        /// Updates entity in repository.
        /// </summary>
        /// <param name="key">The key to perform update.</param>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>Updates entity to repository.</returns>
        Task UpdateAsync(string key, TEntity entity);
    }
}
