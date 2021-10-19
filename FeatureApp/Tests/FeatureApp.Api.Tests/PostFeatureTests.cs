namespace FeatureApp.Api.Tests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Tests;
    using FeatureApp.Common.Models;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class PostFeatureTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly FeatureSteps steps;

        public PostFeatureTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.steps = new FeatureSteps(this.factory.CreateClient());
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
        public async Task ShouldReturnBadRequestWhenEmailIsNotValidFormatForPostFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIPostFeature(new CreateFeatureRequest
            {
                Email = "invalidformat",
                FeatureName = "test",
                Enable = true,
            });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "invalidformat is not in valid email format.");
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
