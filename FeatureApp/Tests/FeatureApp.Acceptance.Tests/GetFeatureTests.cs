namespace FeatureApp.Acceptance.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;
    using FeatureApp.Common.Tests;
    using Xunit;

    public class GetFeatureTests
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = TestConfiguration.GetEndpoint(),
        };

        private readonly FeatureSteps steps;

        public GetFeatureTests()
        {
            this.steps = new FeatureSteps(Client);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailNotProvidedForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest());
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The Email field is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenFeatureNameNotProvidedForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = TestConstants.UserEmail });
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The FeatureName field is required.");
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenEmailIsNotValidFormatForGetFeature()
        {
            await this.steps.GivenISetupService();
            await this.steps.WhenIGetFeature(new GetFeatureRequest() { Email = "invalidformat", FeatureName = TestConstants.FeatureName });
            await this.steps.ThenResponseShouldContains(HttpStatusCode.BadRequest, "The Email field is not a valid e-mail address.");
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
