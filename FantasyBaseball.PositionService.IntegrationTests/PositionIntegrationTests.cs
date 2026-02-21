using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Models;
using Xunit;

namespace FantasyBaseball.PositionService.IntegrationTests;

public class PositionIntegrationTests(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
{
  private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

  [Fact]
  public async Task GetPositionsTest()
  {
    var repsonse = await fixture.Client.GetAsync("/api/v1/position");
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
  public async Task GetSimpleTests(string url)
  {
    var httpResponse = await fixture.Client.GetAsync(url);
    Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
  }
}