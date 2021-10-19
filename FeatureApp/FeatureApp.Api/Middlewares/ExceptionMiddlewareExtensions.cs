namespace FeatureApp.Api.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionMiddlewareExtensions
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddlewareExtensions> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddlewareExtensions"/> class.
        /// </summary>
        /// <param name="next">The next middleware to be executed.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ExceptionMiddlewareExtensions(RequestDelegate next, ILoggerFactory loggerFactory)
            : this(next, loggerFactory.CreateLogger<ExceptionMiddlewareExtensions>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddlewareExtensions"/> class.
        /// </summary>
        /// <param name="next">The next middleware to be executed.</param>
        /// <param name="logger">The logger.</param>
        public ExceptionMiddlewareExtensions(RequestDelegate next, ILogger<ExceptionMiddlewareExtensions> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// Invokes and catch all unhandled exceptions.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns>The executed invocation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                this.logger.LogError(error, $"{context.TraceIdentifier} - {error.Message}");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(
                    $"An unhandled error has occured. Please provide this trace identifier({context.TraceIdentifier}) to support team.");
            }
        }
    }
}