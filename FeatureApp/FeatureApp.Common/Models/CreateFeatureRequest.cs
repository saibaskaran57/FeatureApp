﻿namespace FeatureApp.Common.Models
{
    using Newtonsoft.Json;

    public sealed class CreateFeatureRequest
    {
        /// <summary>
        /// Gets or sets feature name for create.
        /// </summary>
        [JsonProperty("featureName")]
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets feature toggle.
        /// </summary>
        [JsonProperty("enable")]
        public bool Enable { get; set; }
    }
}
