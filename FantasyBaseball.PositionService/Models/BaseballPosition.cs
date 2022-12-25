using System.Collections.Generic;
using FantasyBaseball.PositionService.Models.Enums;

namespace FantasyBaseball.PositionService.Models
{
  /// <summary>All of the information that makes up a baseball position.</summary>
  public class BaseballPosition
  {
    /// <summary>The position's code.</summary>
    public string Code { get; set; }

    /// <summary>The full name of the position.</summary>
    public string FullName { get; set; }

    /// <summary>The type of player (batter or pitcher) that plays this position.</summary>
    public PlayerType PlayerType { get; set; }

    /// <summary>The order this position should be sorted in.</summary>
    public int SortOrder { get; set; }

    /// <summary>Additional positions that this position is eligible for.</summary>
    public List<BaseballPosition> AdditionalPositions { get; set; } = new List<BaseballPosition>();
  }
}