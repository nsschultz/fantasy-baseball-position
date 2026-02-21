namespace FantasyBaseball.PositionService.Database.Entities;

/// <summary>Additional positions that the parent position is eligible for.</summary>
public class AdditionalPositionEntity
{
  /// <summary>The parent's position code.</summary>
  public string ParentCode { get; set; }

  /// <summary>The child's position code.</summary>
  public string ChildCode { get; set; }

  /// <summary>The position of the parent.</summary>
  public PositionEntity ParentPosition { get; set; }

  /// <summary>The position of the child.</summary>
  public PositionEntity ChildPosition { get; set; }
}