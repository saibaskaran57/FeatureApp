namespace FeatureApp.Infrastructure.Tests
{
    using System;
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;
    using FeatureApp.Common.Tests;
    using FeatureApp.Infrastructure.Tests.Steps;
    using Xunit;

    public class FeatureServiceTests
    {
        private readonly FeatureServiceSteps steps;

        public FeatureServiceTests()
        {
            this.steps = new FeatureServiceSteps();
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionWhenRequestIsNullForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => this.steps.WhenICreateFeatureAsync(null));
        }

        [Fact]
        public async Task ShouldSuccessfullyCreateFeatureForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryIsEmpty();
            await this.steps.WhenICreateFeatureAsync(new CreateFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName,
                Enable = false,
            });
            await this.steps.ThenShouldCreateFeatureSuccessfully();
        }

        [Fact]
        public async Task ShouldSuccessfullyUpdateFeatureWhenFeatureNameIsCaseSensitiveForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenICreateFeatureAsync(new CreateFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName.ToUpperInvariant(),
                Enable = false,
            });
            await this.steps.ThenShouldUpdateFeatureSuccessfully();
        }

        [Fact]
        public async Task ShouldSuccessfullyUpdateFeatureForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenICreateFeatureAsync(new CreateFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName,
                Enable = false,
            });
            await this.steps.ThenShouldUpdateFeatureSuccessfully();
        }

        [Fact]
        public async Task ShouldNotUpdateFeatureWhenEmailHasNoAccessForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenICreateFeatureAsync(new CreateFeatureRequest()
            {
                Email = "unknown@hotmail.com",
                FeatureName = TestConstants.FeatureName,
                Enable = true,
            });
            await this.steps.ThenShouldNotModifyFeature();
        }

        [Fact]
        public async Task ShouldNotUpdateFeatureWhenToggleIsTheSameForCreateFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenICreateFeatureAsync(new CreateFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName,
                Enable = true,
            });
            await this.steps.ThenShouldNotModifyFeature();
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionWhenRequestIsNullForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => this.steps.WhenIGetFeatureAsync(null));
        }

        [Fact]
        public async Task ShouldSuccessfullyGetFeatureWithRightAccessForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenIGetFeatureAsync(new GetFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName,
            });
            await this.steps.ThenShouldGetFeatureSuccessfully(true);
        }

        [Fact]
        public async Task ShouldSuccessfullyGetFeatureWithRightAccessWhenEmailIsCaseSensitiveForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenIGetFeatureAsync(new GetFeatureRequest()
            {
                Email = TestConstants.UserEmail.ToUpperInvariant(),
                FeatureName = TestConstants.FeatureName,
            });
            await this.steps.ThenShouldGetFeatureSuccessfully(true);
        }


        [Fact]
        public async Task ShouldSuccessfullyGetFeatureWhenFeatureNameIsCaseSensitiveForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenIGetFeatureAsync(new GetFeatureRequest()
            {
                Email = TestConstants.UserEmail.ToUpperInvariant(),
                FeatureName = TestConstants.FeatureName.ToUpperInvariant(),
            });
            await this.steps.ThenShouldGetFeatureSuccessfully(true);
        }

        [Fact]
        public async Task ShouldNotProvideAccessToInvalidUserEmailForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryContainsData();
            await this.steps.WhenIGetFeatureAsync(new GetFeatureRequest()
            {
                Email = "unknown@hotmail.com",
                FeatureName = TestConstants.FeatureName,
            });
            await this.steps.ThenShouldGetFeatureSuccessfully(false);
        }

        [Fact]
        public async Task ShouldReturnNullWhenFeatureNotExistForGetFeatureAsync()
        {
            await this.steps.GivenISetupFeatureService();
            await this.steps.AndRepositoryIsEmpty();
            await this.steps.WhenIGetFeatureAsync(new GetFeatureRequest()
            {
                Email = TestConstants.UserEmail,
                FeatureName = TestConstants.FeatureName,
            });
            await this.steps.ThenShouldResponseBeNull();
        }
    }
}
