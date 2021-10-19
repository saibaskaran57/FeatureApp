namespace FeatureApp.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public sealed class GetFeatureRequest
    {
        /// <summary>
        /// Gets or sets the feature toggle name.
        /// </summary>
        [Required]
        [JsonProperty("featureName")]
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
