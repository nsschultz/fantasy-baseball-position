using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Entities;

namespace FantasyBaseball.PositionService.Services
{
    /// <summary>Service for converting a PositionEntity to a BaseballPosition.</summary>
    public interface IBaseballPositionBuilderService
    {
        /// <summary>Converts a PositionEntity to a BaseballPosition.</summary>
        /// <param name="position">The database values.</param>
        /// <returns>A BaseballPosition based off the database values.</returns>
        BaseballPosition BuildBaseballPosition(PositionEntity position);
    }
}