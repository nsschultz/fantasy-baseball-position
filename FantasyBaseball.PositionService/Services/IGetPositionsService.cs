using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.Common.Models;

namespace FantasyBaseball.PositionService.Services;

/// <summary>Service for retrieving position data.</summary>
public interface IGetPositionsService
{
    /// <summary>Gets all positions from the underlying data source.</summary>
    /// <returns>A list of all positions.</returns>
    Task<List<Position>> GetPositions();
}