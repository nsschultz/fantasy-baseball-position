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
}