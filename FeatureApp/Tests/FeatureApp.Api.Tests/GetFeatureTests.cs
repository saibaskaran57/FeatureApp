namespace FeatureApp.Api.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;
    using FeatureApp.Common.Tests;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class GetFeatureTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly FeatureSteps steps;

        public GetFeatureTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.steps = new FeatureSteps(this.factory.CreateClient());
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailNotProvidedForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest());
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "email query string is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenFeatureNameNotProvidedForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = TestConstants.UserEmail });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "featureName query string is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailIsNotValidFormatForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = "invalidformat", FeatureName = TestConstants.FeatureName });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.BadRequest, "invalidformat is not in valid email format.");
        }

        [Fact]
        public async Task ShouldReturnNotFoundWWhenFeatureDoesNotExistForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = TestConstants.UserEmail, FeatureName = "notexist" });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.NotFound, "notexist is not found.");
        }

        [Fact]
        public async Task ShouldReturnOkWithCanAccessIsTrueForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = TestConstants.UserEmail, FeatureName = TestConstants.FeatureName });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, new GetFeatureResponse { CanAccess = true });
        }

        [Fact]
        public async Task ShouldReturnOkWithCanAccessIsTrueWhenUserEmailIsEncodedForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = WebUtility.UrlEncode(TestConstants.UserEmail), FeatureName = TestConstants.FeatureName });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, new GetFeatureResponse { CanAccess = true });
        }

        [Fact]
        public async Task ShouldReturnOkWithCanAccessIsFalseWhenUserEmailIsNotTheCreatorForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = "unknown@hotmail.com", FeatureName = TestConstants.FeatureName });
            await this.steps.ThenResponseShouldBe(HttpStatusCode.OK, new GetFeatureResponse { CanAccess = false });
        }
    }
}
