namespace FeatureApp.Api.Controllers
{
    using System.Threading.Tasks;
    using FeatureApp.Common;
    using FeatureApp.Common.Models;
    using FeatureApp.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("feature")]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService featureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureController"/> class.
        /// </summary>
        /// <param name="featureService">The feature service to handle requests.</param>
        /// <param name="logger">Logger for troubleshooting.</param>
        public FeatureController(IFeatureService featureService)
        {
            Guard.RequiresNotNull(featureService, nameof(featureService));

            this.featureService = featureService;
        }

        /// <summary>
        /// Gets feature toggle accessiblity when requested via email and feature name.
        /// </summary>
        /// <param name="request">Request to get toggle feature.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetFeatureRequest request)
        {
            if (request == null)
            {
                return this.BadRequest($"{nameof(request)} is required.");
            }

            var response = await this.featureService.GetFeatureAsync(request);

            return response != null
                ? this.Ok(response)
                : this.StatusCode(StatusCodes.Status404NotFound, $"{request.FeatureName} is not found.");
        }

        /// <summary>
        /// Creates/Modifies feature toggles.
        /// </summary>
        /// <param name="request">Request to toggle feature.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFeatureRequest request)
        {
            if (request == null)
            {
                return this.BadRequest($"{nameof(request)} is required.");
            }

            var response = await this.featureService.CreateOrUpdateFeatureAsync(request);

            return response != null && response.Result == ResultCode.Success
                ? this.Ok()
                : this.StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
