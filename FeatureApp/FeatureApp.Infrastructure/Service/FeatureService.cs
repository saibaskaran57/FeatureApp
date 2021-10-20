namespace FeatureApp.Infrastructure.Service
{
    using System;
    using System.Threading.Tasks;
    using FeatureApp.Common;
    using FeatureApp.Common.Models;
    using FeatureApp.Core;

    public sealed class FeatureService : IFeatureService
    {
        private readonly IRepository<Feature> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureService"/> class.
        /// </summary>
        /// <param name="repository">The feature repository. </param>
        public FeatureService(IRepository<Feature> repository)
        {
            Guard.RequiresNotNull(repository, nameof(repository));

            this.repository = repository;
        }

        /// <inheritdoc/>
        public async Task<CreateFeatureResponse> CreateOrUpdateFeatureAsync(CreateFeatureRequest request)
        {
            Guard.RequiresNotNull(request, nameof(request));

            var result = await this.repository.GetAsync(request.FeatureName.ToLowerInvariant());

            return result == null
                ? await this.CreateAsync(request)
                : await this.UpdateAsync(request, result);
        }

        /// <inheritdoc/>
        public async Task<GetFeatureResponse> GetFeatureAsync(GetFeatureRequest request)
        {
            Guard.RequiresNotNull(request, nameof(request));

            var result = await this.repository.GetAsync(request.FeatureName.ToLowerInvariant());

            return result != null
                ? new GetFeatureResponse
                {
                    CanAccess = IsAuthorized(request.Email, result.Email) && result.Enable,
                }
                : null;
        }

        private static bool IsAuthorized(string sourceEmail, string targetEmail)
        {
            return !string.IsNullOrWhiteSpace(targetEmail)
                && targetEmail.Equals(sourceEmail, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsToggled(bool isSourceFeatureEnabled, bool isTargetFeatureEnabled)
        {
            return isSourceFeatureEnabled != isTargetFeatureEnabled;
        }

        private async Task<CreateFeatureResponse> UpdateAsync(CreateFeatureRequest request, Feature result)
        {
            if (!IsAuthorized(request.Email, result.Email) || !IsToggled(request.Enable, result.Enable))
            {
                return new CreateFeatureResponse() { Result = ResultCode.NotModified };
            }

            await this.repository.UpdateAsync(
                request.FeatureName,
                new Feature(request.FeatureName, request.Email, request.Enable));

            return new CreateFeatureResponse { Result = ResultCode.Success };
        }

        private async Task<CreateFeatureResponse> CreateAsync(CreateFeatureRequest request)
        {
            await this.repository.SaveAsync(new Feature(request.FeatureName, request.Email, request.Enable));

            return new CreateFeatureResponse() { Result = ResultCode.Success };
        }
    }
}
