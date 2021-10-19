namespace FeatureApp.Acceptance.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;
    using FeatureApp.Common.Tests;
    using Xunit;

    public class PostFeatureTests
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = TestConfiguration.GetEndpoint(),
        };

        private readonly FeatureSteps steps;

        public PostFeatureTests()
        {
            this.steps = new FeatureSteps(Client);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailNotProvidedForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest());
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "Email is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenFeatureNameNotProvidedForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest() { Email = TestConstants.UserEmail });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "FeatureName is required.");
        }

        [Fact]
        public async Task ShouldReturnOkForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = Guid.NewGuid().ToString(),
                FeatureName = $"test-{Guid.NewGuid()}",
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, string.Empty);
        }

        [Fact]
        public async Task ShouldReturnOkWhenFeatureToggledFromTrueToFalseForPostFeature()
        {
            CreateRequest(out string email, out string featureName);

            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, string.Empty);
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
                Enable = false,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, string.Empty);
        }

        [Fact]
        public async Task ShouldReturnNotModifiedWhenFeatureToggleIsTheSameForPostFeature()
        {
            CreateRequest(out string email, out string featureName);

            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, string.Empty);
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.NotModified, string.Empty);
        }

        [Fact]
        public async Task ShouldReturnNotModifiedWhenNoAccessForPostFeature()
        {
            CreateRequest(out string email, out string featureName);

            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, string.Empty);
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = "unknown@hotmail.com",
                FeatureName = featureName,
                Enable = false,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.NotModified, string.Empty);
        }

        private static void CreateRequest(out string email, out string featureName)
        {
            email = Guid.NewGuid().ToString();
            featureName = $"test-{Guid.NewGuid()}";
        }
    }
}
