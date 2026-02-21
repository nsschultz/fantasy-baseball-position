using System.Threading.Tasks;
using FantasyBaseball.PositionService.Controllers.V1;
using FantasyBaseball.PositionService.Models;
using FantasyBaseball.PositionService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Controllers.V1;

public class PositionControllerTest
{
  [Fact]
  public async Task GetPositionsTest()
  {
    var getService = new Mock<IGetPositionsService>();
    getService.Setup(o => o.GetPositions()).ReturnsAsync([new BaseballPosition()]);
    Assert.NotEmpty(await new PositionController(getService.Object).GetPositions());
  }
}