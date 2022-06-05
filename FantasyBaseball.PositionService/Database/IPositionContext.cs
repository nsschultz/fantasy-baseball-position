using FantasyBaseball.PositionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.PositionService.Database
{
    /// <summary>The context object for positions and their related entities.</summary>
    public interface IPositionContext
    {
        /// <summary>A collection of child positions.</summary>
        DbSet<AdditionalPositionEntity> AdditionalPositions { get; set; }

        /// <summary>A collection of positions.</summary>
        DbSet<PositionEntity> Positions { get; set; }
    }
}