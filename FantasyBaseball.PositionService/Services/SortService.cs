using System.Collections.Generic;
using System.Linq;
using FantasyBaseball.Common.Models;

namespace FantasyBaseball.PositionService.Services
{
    /// <summary>Service for sorting the positions.</summary>
    public class SortService : ISortService
    {
        /// <summary>Sorts the collection of positions.</summary>
        /// <param name="positions">All of the positions to sort.</param>
        /// <returns>The sorted collection of positions.</returns>
        public List<BaseballPosition> SortPositions(List<BaseballPosition> positions) => positions.OrderBy(p => p.SortOrder).ToList();
    }
}