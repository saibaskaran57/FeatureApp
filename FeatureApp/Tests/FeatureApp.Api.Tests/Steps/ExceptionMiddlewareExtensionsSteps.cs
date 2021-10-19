namespace FeatureApp.Api.Tests.Steps
{
    using FeatureApp.Api.Middlewares;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class ExceptionMiddlewareExtensionsSteps
    {
        private Mock<ILogger<ExceptionMiddlewareExtensions>> logger;
        private ExceptionMiddlewareExtensions middleware;
        private DefaultHttpContext context;

        public async Task<ExceptionMiddlewareExtensionsSteps> GivenISetupExceptionMiddlewareWithError()
        {
            var errorMessage = "An error has occured";
            this.context = new DefaultHttpContext();
            this.context.Response.Body = new MemoryStream();
            this.context.Request.Path = "/";

            this.logger = new Mock<ILogger<ExceptionMiddlewareExtensions>>();

            this.middleware = new ExceptionMiddlewareExtensions(next: (innerHttpContext) =>
            {
                throw new InvalidOperationException(errorMessage);
            }, this.logger.Object);

            return await Task.FromResult(this);
        }

        public async Task<ExceptionMiddlewareExtensionsSteps> WhenIExecuteExceptionMiddleware()
        {
            await this.middleware.Invoke(context);

            return await Task.FromResult(this);
        }

        public async Task<ExceptionMiddlewareExtensionsSteps> ThenShouldGetErrorMessage()
        {
            var expectedErrorMessage = $"An unhandled error has occured. Please provide this trace identifier({this.context.TraceIdentifier}) to support team.";
            this.context.Response.Body.Seek(0, SeekOrigin.Begin);

            var body = await new StreamReader(this.context.Response.Body).ReadToEndAsync();

            Assert.Equal(expectedErrorMessage, body);

            return await Task.FromResult(this);
        }
    }
}
