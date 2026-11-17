using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.PositionService.Database.Repositories;

/// <summary>Repository for position data access operations.</summary>
/// <param name="context">The EF Core database context for positions.</param>
public class PositionRepository(IPositionContext context) : IPositionRepository
{
    /// <summary>Gets all positions from the database with their relationships eager-loaded.</summary>
    /// <returns>A list of all positions including child and parent position relationships.</returns>
    public async Task<List<PositionEntity>> GetPositions() =>
        await context.Positions
            .AsSplitQuery()
            .Include(p => p.ChildPositions)
            .Include(p => p.ParentPositions)
            .ToListAsync();
}