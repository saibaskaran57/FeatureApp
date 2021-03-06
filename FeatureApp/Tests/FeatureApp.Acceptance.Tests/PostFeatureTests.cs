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
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The Email field is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenFeatureNameNotProvidedForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest() { Email = TestConstants.UserEmail });
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The FeatureName field is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailIsNotValidFormatForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = "invalidformat",
                FeatureName = "test",
                Enable = true,
            });
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The Email field is not a valid e-mail address.");
        }

        [Fact]
        public async Task ShouldReturnOkForPostFeature()
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
            email = $"{Guid.NewGuid()}@hotmail.com";
            featureName = $"test-{Guid.NewGuid()}";
        }

    }
}
