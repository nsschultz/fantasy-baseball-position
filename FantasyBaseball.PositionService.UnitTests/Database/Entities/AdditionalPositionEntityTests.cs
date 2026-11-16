using FantasyBaseball.PositionService.Database.Entities;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Database.Entities;

/// <summary>Tests for the <see cref="AdditionalPositionEntity"/>.</summary>
public class AdditionalPositionEntityTests
{
  /// <summary>Verifies that new instances have expected default values.</summary>
  [Fact]
  public void HasDefaultValues()
  {
    var entity = new AdditionalPositionEntity();
    Assert.Null(entity.ChildCode);
    Assert.Null(entity.ParentCode);
    Assert.Null(entity.ChildPosition);
    Assert.Null(entity.ParentPosition);
  }
}