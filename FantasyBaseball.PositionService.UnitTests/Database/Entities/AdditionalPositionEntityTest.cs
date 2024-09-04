using FantasyBaseball.PositionService.Database.Entities;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Database.Entities
{
  public class AdditionalPositionEntityTest
  {
    [Fact]
    public void DefaultsSetTest()
    {
      var obj = new AdditionalPositionEntity();
      Assert.Null(obj.ChildCode);
      Assert.Null(obj.ParentCode);
      Assert.Null(obj.ChildPosition);
      Assert.Null(obj.ParentPosition);
    }
  }
}