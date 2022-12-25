using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Models;

namespace FantasyBaseball.PositionService.Services
{
  /// <summary>Service for getting positions.</summary>
  public interface IGetPositionsService
  {
    /// <summary>Gets the positions from the underlying source.</summary>
    /// <returns>A list of the positions.</returns>
    Task<List<BaseballPosition>> GetPositions();
  }
}