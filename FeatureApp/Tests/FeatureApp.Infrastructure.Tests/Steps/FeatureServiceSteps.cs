namespace FeatureApp.Infrastructure.Tests.Steps
{
    using System.Threading.Tasks;
    using FeatureApp.Common.Models;
    using FeatureApp.Common.Tests;
    using FeatureApp.Core;
    using FeatureApp.Infrastructure.Service;
    using Moq;
    using Xunit;

    public class FeatureServiceSteps
    {
        private Mock<IRepository<Feature>> repository;
        private FeatureService service;
        private object response;

        public async Task<FeatureServiceSteps> GivenISetupFeatureService()
        {
            this.repository = new Mock<IRepository<Feature>>();
            this.service = new FeatureService(this.repository.Object);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> WhenICreateFeatureAsync(CreateFeatureRequest request)
        {
            this.response = await this.service.CreateOrUpdateFeatureAsync(request);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> AndRepositoryIsEmpty()
        {
            this.repository.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<Feature>(null));
            this.repository.Setup(x => x.SaveAsync(It.IsAny<Feature>()));

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> AndRepositoryContainsData()
        {
            this.repository.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(new Feature(TestConstants.FeatureName, TestConstants.UserEmail, true)));
            this.repository.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Feature>()));

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> WhenIGetFeatureAsync(GetFeatureRequest request)
        {
            this.response = await this.service.GetFeatureAsync(request);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> ThenShouldCreateFeatureSuccessfully()
        {
            var actual = this.response as CreateFeatureResponse;
            this.repository.Verify(x => x.SaveAsync(It.IsAny<Feature>()), Times.Once);

            Assert.Equal(ResultCode.Success, actual.Result);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> ThenShouldUpdateFeatureSuccessfully()
        {
            var actual = this.response as CreateFeatureResponse;
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Feature>()), Times.Once);

            Assert.Equal(ResultCode.Success, actual.Result);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> ThenShouldNotModifyFeature()
        {
            var actual = this.response as CreateFeatureResponse;

            Assert.Equal(ResultCode.NotModified, actual.Result);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> ThenShouldGetFeatureSuccessfully(bool canAccess)
        {
            var actual = this.response as GetFeatureResponse;

            Assert.Equal(canAccess, actual.CanAccess);

            return await Task.FromResult(this);
        }

        public async Task<FeatureServiceSteps> ThenShouldResponseBeNull()
        {
            Assert.Null(this.response);

            return await Task.FromResult(this);
        }
    }
}
