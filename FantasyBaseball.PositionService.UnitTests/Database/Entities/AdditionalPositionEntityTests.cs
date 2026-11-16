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

    /// <summary>Verifies that properties can be set after construction.</summary>
    [Fact]
    public void CanSetProperties()
    {
        var childPosition = new PositionEntity { Code = "1B", FullName = "First Baseman" };
        var parentPosition = new PositionEntity { Code = "IF", FullName = "Infielder" };

        var entity = new AdditionalPositionEntity
        {
            ChildCode = "1B",
            ParentCode = "IF",
            ChildPosition = childPosition,
            ParentPosition = parentPosition
        };

        Assert.Equal("1B", entity.ChildCode);
        Assert.Equal("IF", entity.ParentCode);
        Assert.Equal(childPosition, entity.ChildPosition);
        Assert.Equal(parentPosition, entity.ParentPosition);
    }
}