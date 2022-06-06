using System.Collections.Generic;
using FantasyBaseball.Common.Models;

namespace FantasyBaseball.PositionService.Services
{
    /// <summary>Service for sorting the positions.</summary>
    public interface ISortService
    {
        /// <summary>Sorts the collection of positions.</summary>
        /// <param name="positions">All of the positions to sort.</param>
        /// <returns>The sorted collection of positions.</returns>
        List<BaseballPosition> SortPositions(List<BaseballPosition> positions);
    }
}