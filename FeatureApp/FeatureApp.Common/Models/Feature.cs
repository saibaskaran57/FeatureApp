namespace FeatureApp.Common.Models
{
    using Newtonsoft.Json;

    public sealed class Feature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="enable">The feature to be toggled.</param>
        public Feature(string featureName, string email, bool enable)
        {
            this.Id = featureName.ToLowerInvariant();
            this.FeatureName = featureName.ToLowerInvariant();
            this.Email = email.ToLowerInvariant();
            this.Enable = enable;
        }

        /// <summary>
        /// Gets the feature id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Gets the feature name.
        /// </summary>
        [JsonProperty("featureName")]
        public string FeatureName { get; private set; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; private set; }

        /// <summary>
        /// Gets a value indicating whether feature is toggle.
        /// </summary>
        [JsonProperty("enable")]
        public bool Enable { get; private set; }
    }
}
