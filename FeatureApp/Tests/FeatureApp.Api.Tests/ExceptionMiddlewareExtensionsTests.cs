namespace FeatureApp.Api.Tests
{
    using FeatureApp.Api.Tests.Steps;
    using System.Threading.Tasks;
    using Xunit;

    public class ExceptionMiddlewareExtensionsTests
    {
        private readonly ExceptionMiddlewareExtensionsSteps steps;

        public ExceptionMiddlewareExtensionsTests()
        {
            this.steps = new ExceptionMiddlewareExtensionsSteps();
        }

        [Fact]
        public async Task ShouldReturnErrorMessageWhenUnhandledExceptionOccured()
        {
            await this.steps.GivenISetupExceptionMiddlewareWithError();
            await this.steps.WhenIExecuteExceptionMiddleware();
            await this.steps.ThenShouldGetErrorMessage();
        }
    }
}
