namespace FeatureApp.Core
{
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;

    public interface IFeatureService
    {
        /// <summary>
        /// Creates or updates feature toggles.
        /// </summary>
        /// <param name="request">The feature to be created or updated.</param>
        /// <returns>The feature created response.</returns>
        Task<CreateFeatureResponse> CreateOrUpdateFeatureAsync(CreateFeatureRequest request);

        /// <summary>
        /// Gets the feature toggles based on user access.
        /// </summary>
        /// <param name="request">The feature to be queried.</param>
        /// <returns>The get feature response.</returns>
        Task<GetFeatureResponse> GetFeatureAsync(GetFeatureRequest request);
    }
}
