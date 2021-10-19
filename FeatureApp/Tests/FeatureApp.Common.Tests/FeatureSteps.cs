namespace FeatureApp.Common.Tests
{
    using FeatureApp.Common.Models;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class FeatureSteps
    {
        private const string FeatureEndpoint = "/feature";
        private string endpoint;
        private HttpResponseMessage response;
        private readonly HttpClient client;

        public FeatureSteps(HttpClient httpClient)
        {
            this.client = httpClient;
        }

        public async Task<FeatureSteps> GivenISetupService()
        {
            this.endpoint = FeatureEndpoint;

            return await Task.FromResult(this);
        }

        public async Task<FeatureSteps> WhenIGetFeature(GetFeatureRequest request)
        {
            this.response = await this.client.GetAsync($"{endpoint}?email={request.Email}&featureName={request.FeatureName}");

            return await Task.FromResult(this);
        }

        public async Task<FeatureSteps> WhenIPostFeature(CreateFeatureRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            this.response = await this.client.PostAsync(endpoint, content);

            return await Task.FromResult(this);
        }

        public async Task<FeatureSteps> ThenResponseShouldBe(HttpStatusCode code, string response)
        {
            var content = await this.response.Content.ReadAsStringAsync();

            Assert.Equal(code, this.response.StatusCode);
            Assert.Equal(response, content);

            return await Task.FromResult(this);
        }

        public async Task<FeatureSteps> ThenResponseShouldBe<T>(HttpStatusCode code, T response)
        {
            var content = await this.response.Content.ReadAsStringAsync();

            Assert.Equal(code, this.response.StatusCode);
            Assert.NotStrictEqual(response, JsonConvert.DeserializeObject<T>(content));

            return await Task.FromResult(this);
        }
    }
}
