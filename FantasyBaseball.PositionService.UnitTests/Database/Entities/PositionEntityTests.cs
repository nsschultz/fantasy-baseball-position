using FantasyBaseball.Common.Enums;
using FantasyBaseball.PositionService.Database.Entities;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Database.Entities;

/// <summary>Tests for the <see cref="PositionEntity"/>.</summary>
public class PositionEntityTests
{
    /// <summary>Verifies that new instances have expected default values.</summary>
    [Fact]
    public void HasDefaultValues()
    {
        var entity = new PositionEntity { Code = "test", FullName = "test" };
        Assert.Equal(PlayerType.U, entity.PlayerType);
        Assert.Equal(0, entity.SortOrder);
        Assert.Empty(entity.ChildPositions);
        Assert.Empty(entity.ParentPositions);
    }
}