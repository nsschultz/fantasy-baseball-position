using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.PositionService.Database.Repositories;

/// <summary>Service for getting positions.</summary>
/// <param name="context">The position context.</param>
public class PositionRepository(IPositionContext context) : IPositionRepository
{
  /// <summary>Gets all of the positions in the database.</summary>
  /// <returns>All of the positions in the database.</returns>
  public async Task<List<PositionEntity>> GetPositions() =>
    await context.Positions
      .Include(p => p.ChildPositions)
      .Include(p => p.ParentPositions)
      .ToListAsync();
}