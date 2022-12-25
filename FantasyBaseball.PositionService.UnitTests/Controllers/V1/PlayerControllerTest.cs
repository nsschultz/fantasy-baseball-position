using System.Collections.Generic;
using FantasyBaseball.PositionService.Models;
using FantasyBaseball.PositionService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.Controllers.V1.UnitTests
{
  public class PositionControllerTest
  {
    [Fact]
    public async void GetPositionsTest()
    {
      var getService = new Mock<IGetPositionsService>();
      getService.Setup(o => o.GetPositions()).ReturnsAsync(new List<BaseballPosition> { new BaseballPosition() });
      Assert.NotEmpty((await new PositionController(getService.Object).GetPositions()));
    }
  }
}