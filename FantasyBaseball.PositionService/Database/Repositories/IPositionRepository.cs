using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database.Entities;

namespace FantasyBaseball.PositionService.Database.Repositories
{
  /// <summary>Repo for CRUD functionality regarding to positions.</summary>
  public interface IPositionRepository
  {
    /// <summary>Gets all of the positions in the database.</summary>
    /// <returns>All of the positions in the database.</returns>
    Task<List<PositionEntity>> GetPositions();
  }
}