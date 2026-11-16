using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.Common.Models;
using Xunit;

namespace FantasyBaseball.PositionService.IntegrationTests;

/// <summary>Integration tests for Position API endpoints.</summary>
public class PositionIntegrationTests(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    /// <summary>Verifies that GetPositions endpoint returns all positions correctly sorted.</summary>
    [Fact]
    public async Task GetPositions_ReturnsAllPositionsSorted()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var positions = await JsonSerializer.DeserializeAsync<List<Position>>(await response.Content.ReadAsStreamAsync(), _options);

        Assert.Equal(18, positions.Count);

        // Verify no nested additional positions (max 1 level deep)
        foreach (var position in positions)
        {
            if (position.AdditionalPositions != null)
            {
                foreach (var additionalPosition in position.AdditionalPositions)
                {
                    Assert.DoesNotContain(additionalPosition.AdditionalPositions ?? new(), ap => ap.AdditionalPositions?.Count > 0);
                }
            }
        }
    }

    /// <summary>Verifies that all positions have required fields populated.</summary>
    [Fact]
    public async Task GetPositions_AllPositionsHaveRequiredFields()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        var positions = await JsonSerializer.DeserializeAsync<List<Position>>(await response.Content.ReadAsStreamAsync(), _options);

        foreach (var position in positions)
        {
            Assert.NotNull(position.Code);
            Assert.NotNull(position.FullName);
            Assert.True(position.SortOrder >= 0 || position.SortOrder == int.MaxValue);
        }
    }

    /// <summary>Verifies that positions are sorted by SortOrder ascending.</summary>
    [Fact]
    public async Task GetPositions_AreSortedBySortOrder()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        var positions = await JsonSerializer.DeserializeAsync<List<Position>>(await response.Content.ReadAsStreamAsync(), _options);

        var sortedBySortOrder = positions.OrderBy(p => p.SortOrder).ToList();

        for (int i = 0; i < positions.Count; i++)
        {
            Assert.Equal(sortedBySortOrder[i].Code, positions[i].Code);
        }
    }

    /// <summary>Verifies that positions have both batter and pitcher types.</summary>
    [Fact]
    public async Task GetPositions_IncludesBothPlayerTypes()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        var positions = await JsonSerializer.DeserializeAsync<List<Position>>(await response.Content.ReadAsStreamAsync(), _options);

        var hasBatters = positions.Any(p => p.PlayerType == PlayerType.B);
        var hasPitchers = positions.Any(p => p.PlayerType == PlayerType.P);
        var hasUnknown = positions.Any(p => p.PlayerType == PlayerType.U);

        Assert.True(hasBatters);
        Assert.True(hasPitchers);
        Assert.True(hasUnknown);
    }

    /// <summary>Verifies that additional positions contain valid position references.</summary>
    [Fact]
    public async Task GetPositions_AdditionalPositionsAreValid()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        var positions = await JsonSerializer.DeserializeAsync<List<Position>>(await response.Content.ReadAsStreamAsync(), _options);
        var allPositionCodes = positions.Select(p => p.Code).ToHashSet();

        var additionalPositions = positions.SelectMany(p => p.AdditionalPositions).ToList();

        foreach (var additionalPosition in additionalPositions)
        {
            Assert.True(allPositionCodes.Contains(additionalPosition.Code), $"Additional position {additionalPosition.Code} not found in main positions");
        }
    }

    /// <summary>Verifies that health check endpoint is accessible.</summary>
    [Fact]
    public async Task Health_Endpoint_ReturnsOk()
    {
        var response = await fixture.Client.GetAsync("/api/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>Verifies that Swagger documentation endpoint is accessible.</summary>
    [Fact]
    public async Task Swagger_Endpoint_ReturnsOk()
    {
        var response = await fixture.Client.GetAsync("/api/swagger/index.html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>Verifies that API responds with correct Content-Type header.</summary>
    [Fact]
    public async Task GetPositions_ReturnsJsonContentType()
    {
        var response = await fixture.Client.GetAsync("/api/v1/position");
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    /// <summary>Verifies that invalid endpoints return 404.</summary>
    [Fact]
    public async Task InvalidEndpoint_Returns404()
    {
        var response = await fixture.Client.GetAsync("/api/v1/invalid");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}