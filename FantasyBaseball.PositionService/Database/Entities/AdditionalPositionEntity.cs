namespace FantasyBaseball.PositionService.Database.Entities;

/// <summary>
/// Additional positions that the parent position is eligible for.
/// Represents a relationship between a parent position and child position in the position hierarchy.
/// </summary>
public class AdditionalPositionEntity
{
    /// <summary>The parent position's code.</summary>
    public string ParentCode { get; set; }

    /// <summary>The child position's code.</summary>
    public string ChildCode { get; set; }

    /// <summary>The parent position entity associated with this relationship.</summary>
    public PositionEntity ParentPosition { get; set; }

    /// <summary>The child position entity associated with this relationship.</summary>
    public PositionEntity ChildPosition { get; set; }
}