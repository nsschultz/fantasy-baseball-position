using FantasyBaseball.PositionService.Models.Enums;
using Xunit;

namespace FantasyBaseball.PositionService.Database.Entities.UnitTests
{
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
}