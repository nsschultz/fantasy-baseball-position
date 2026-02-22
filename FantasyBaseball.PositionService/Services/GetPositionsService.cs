using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Models;

namespace FantasyBaseball.PositionService.Services;

/// <summary>Service for getting positions.</summary>
/// <param name="mapper">Instance of the auto mapper.</param>
/// <param name="positionRepo">Repo for CRUD functionality regarding to positions.</param>
public class GetPositionsService(IMapper mapper, IPositionRepository positionRepo) : IGetPositionsService
{
  /// <summary>Gets the positions from the underlying source.</summary>
  /// <returns>A list of the positions.</returns>
  public async Task<List<BaseballPosition>> GetPositions()
  {
    var positions = await positionRepo.GetPositions();
    return [.. positions
      .Select(mapper.Map<PositionEntity, BaseballPosition>)
      .OrderBy(p => p.SortOrder)];
  }
}