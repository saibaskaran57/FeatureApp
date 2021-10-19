namespace FeatureApp.Common.Models
{
    public sealed class GetFeatureRequest
    {
        /// <summary>
        /// Gets or sets the feature toggle name.
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }
    }
}
