using System;

namespace FeatureApp.Acceptance.Tests
{
    public static class TestConfiguration
    {
        public static Uri GetEndpoint()
        {
            return new Uri(Environment.GetEnvironmentVariable("HOST_URL") ?? "https://featureapp.azurewebsites.net/");
        }
    }
}
