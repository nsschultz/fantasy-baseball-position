using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;

namespace FantasyBaseball.PositionService.Services;

/// <summary>Service for getting positions.</summary>
/// <param name="mapper">Instance of the auto mapper.</param>
/// <param name="positionRepo">Repo for CRUD functionality regarding to positions.</param>
public class GetPositionsService(IMapper mapper, IPositionRepository positionRepo) : IGetPositionsService
{
  /// <summary>Gets the positions from the underlying source.</summary>
  /// <returns>A list of the positions.</returns>
  public async Task<List<Position>> GetPositions()
  {
    var positions = await positionRepo.GetPositions();
    return [.. positions
      .Select(mapper.Map<PositionEntity, Position>)
      .OrderBy(p => p.SortOrder)];
  }
}