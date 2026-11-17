using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;

namespace FantasyBaseball.PositionService.Services;

/// <summary>Service for retrieving position data.</summary>
/// <param name="mapper">AutoMapper instance for mapping entities to domain models.</param>
/// <param name="positionRepo">Repository for accessing position data from the database.</param>
public class GetPositionsService(IMapper mapper, IPositionRepository positionRepo) : IGetPositionsService
{
    /// <summary>Gets all positions from the underlying data source, sorted by sort order.</summary>
    /// <returns>A list of all positions, sorted by sort order.</returns>
    public async Task<List<Position>> GetPositions()
    {
        var positions = await positionRepo.GetPositions();
        return [.. positions
            .Select(mapper.Map<PositionEntity, Position>)
            .OrderBy(p => p.SortOrder)];
    }
}