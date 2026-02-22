using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Models.Enums;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Database.Entities;

public class PositionEntityTest
{
  [Fact]
  public void DefaultsSetTest()
  {
    var obj = new PositionEntity();
    Assert.Null(obj.Code);
    Assert.Null(obj.FullName);
    Assert.Equal(PlayerType.U, obj.PlayerType);
    Assert.Equal(0, obj.SortOrder);
    Assert.Empty(obj.ChildPositions);
  }
}