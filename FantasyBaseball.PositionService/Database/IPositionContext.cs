using System.Threading.Tasks;
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

        /// <summary>Starts a new database transaction.</summary>
        Task BeginTransaction();
        
        /// <summary>Commits the database transaction.</summary>
        Task Commit();

        /// <summary>Rolls the database transaction back.</summary>
        Task Rollback();
    }
}