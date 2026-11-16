using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Controllers.V1;
using FantasyBaseball.PositionService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Controllers.V1;

/// <summary>Tests for the <see cref="PositionController"/>.</summary>
public class PositionControllerTests
{
  /// <summary>Verifies that GetPositions returns all positions.</summary>
  [Fact]
  public async Task GetPositions_ReturnsPositions()
  {
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ReturnsAsync([new Position()]);
    var result = await new PositionController(getService.Object).GetPositions();
    Assert.NotEmpty(result);
  }

  /// <summary>Verifies that GetPositions returns empty list when service returns empty.</summary>
  [Fact]
  public async Task GetPositions_WithEmptyService_ReturnsEmptyList()
  {
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ReturnsAsync(new List<Position>());
    
    var result = await new PositionController(getService.Object).GetPositions();
    
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  /// <summary>Verifies that GetPositions returns multiple positions with correct count.</summary>
  [Fact]
  public async Task GetPositions_ReturnMultiplePositions()
  {
    var testPositions = new List<Position>
    {
      new() { Code = "C", FullName = "Catcher", SortOrder = 0 },
      new() { Code = "1B", FullName = "First Baseman", SortOrder = 1 },
      new() { Code = "P", FullName = "Pitcher", SortOrder = 100 }
    };
    
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ReturnsAsync(testPositions);
    
    var result = await new PositionController(getService.Object).GetPositions();
    
    Assert.Equal(3, result.Count);
    Assert.Equal(testPositions, result);
  }

  /// <summary>Verifies that GetPositions propagates service exceptions.</summary>
  [Fact]
  public async Task GetPositions_WhenServiceThrows_PropagatesException()
  {
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ThrowsAsync(new InvalidOperationException("Service error"));
    
    var controller = new PositionController(getService.Object);
    
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.GetPositions());
  }

  /// <summary>Verifies that GetPositions calls the service exactly once.</summary>
  [Fact]
  public async Task GetPositions_CallsServiceOnce()
  {
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ReturnsAsync([new Position()]);
    
    var controller = new PositionController(getService.Object);
    await controller.GetPositions();
    
    getService.Verify(o => o.GetPositions(), Times.Once);
  }
}