using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using FantasyBaseball.PositionService.Models;
using Xunit;

namespace FantasyBaseball.PositionService.IntegrationTests
{
  public class PositionIntegrationTests(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
  {
    private HttpClientFixture _fixture = fixture;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public async void GetPositionsTest()
    {
      var repsonse = await _fixture.Client.GetAsync("/api/v1/position");
      Assert.Equal(HttpStatusCode.OK, repsonse.StatusCode);
      var positions = await JsonSerializer.DeserializeAsync<List<BaseballPosition>>(await repsonse.Content.ReadAsStreamAsync(), _options);
      Assert.Equal(18, positions.Count);
      //Ensure additional positions don't contain additional positions
      positions
        .SelectMany(p => p.AdditionalPositions)
        .ToList()
        .ForEach(ap => Assert.Empty(ap.AdditionalPositions));
    }

    [Theory]
    [InlineData("/api/health")]
    [InlineData("/api/swagger/index.html")]
    public async void GetSimpleTests(string url)
    {
      var httpResponse = await _fixture.Client.GetAsync(url);
      Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }
  }
}