namespace FeatureApp.Api.Controllers
{
    using System.Threading.Tasks;
    using FeatureApp.Common;
    using FeatureApp.Common.Helpers;
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
        /// <param name="email">Feature access email. </param>
        /// <param name="featureName">Feature name. </param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string email, [FromQuery] string featureName)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return this.BadRequest($"{nameof(email)} query string is required.");
            }
            else if (string.IsNullOrWhiteSpace(featureName))
            {
                return this.BadRequest($"{nameof(featureName)} query string is required.");
            }
            else if (!EmailValidator.IsValid(email))
            {
                return this.BadRequest($"{email} is not in valid email format.");
            }

            var response = await this.featureService.GetFeatureAsync(new GetFeatureRequest
            {
                Email = email,
                FeatureName = featureName,
            });

            return response != null
                ? this.Ok(response)
                : this.StatusCode(StatusCodes.Status404NotFound, $"{featureName} is not found.");
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
            else if (string.IsNullOrWhiteSpace(request.Email))
            {
                return this.BadRequest($"{nameof(request.Email)} is required.");
            }
            else if (string.IsNullOrWhiteSpace(request.FeatureName))
            {
                return this.BadRequest($"{nameof(request.FeatureName)} is required.");
            }
            else if (!EmailValidator.IsValid(request.Email))
            {
                return this.BadRequest($"{request.Email} is not in valid email format.");
            }

            var response = await this.featureService.CreateOrUpdateFeatureAsync(request);

            return response != null && response.Result == ResultCode.Success
                ? this.Ok()
                : this.StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
