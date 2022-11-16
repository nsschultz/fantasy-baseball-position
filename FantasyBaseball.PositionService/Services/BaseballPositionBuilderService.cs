using System.Collections.Generic;
using System.Linq;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Entities;

namespace FantasyBaseball.PositionService.Services
{
    /// <summary>Service for converting a PlayerEntity to a BaseballPlayer.</summary>
    public class BaseballPositionBuilderService : IBaseballPositionBuilderService
    {
        /// <summary>Converts a PositionEntity to a BaseballPosition.</summary>
        /// <param name="position">The database values.</param>
        /// <returns>A BaseballPosition based off the database values.</returns>
        public BaseballPosition BuildBaseballPosition(PositionEntity position) =>
            position == null ? new BaseballPosition() : BuildBaseballPosition(position, true);

        private BaseballPosition BuildBaseballPosition(PositionEntity position, bool needAdditionalPos) =>
            new BaseballPosition
            {
                Code = position.Code,
                FullName = position.FullName,
                PlayerType = position.PlayerType,
                SortOrder = position.SortOrder,
                AdditionalPositions = needAdditionalPos
                    ? position.ParentPositions.Select(p => BuildBaseballPosition(p.ChildPosition, false)).ToList()
                    : new List<BaseballPosition>()
            };
    }
}