using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FantasyBaseball.PositionService.IntegrationTests
{
    public class PositionIntegrationTests
    {

        [Theory]
        [InlineData("/api/health")]
        [InlineData("/api/v1/position")]
        [InlineData("/api/v1/position/swagger/index.html")]
        public async void GetTests(string url)
        {
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            var httpResponse = await client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }
    }
}