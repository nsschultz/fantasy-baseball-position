using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Models;

namespace FantasyBaseball.PositionService.Services
{
  /// <summary>Service for getting positions.</summary>
  public class GetPositionsService : IGetPositionsService
  {
    private readonly IMapper _mapper;
    private readonly IPositionRepository _positionRepo;

    /// <summary>Creates a new instance of the service.</summary>
    /// <param name="mapper">Instance of the auto mapper.</param>
    /// <param name="positionRepo">Repo for CRUD functionality regarding to positions.</param>
    public GetPositionsService(IMapper mapper, IPositionRepository positionRepo) => (_mapper, _positionRepo) = (mapper, positionRepo);

    /// <summary>Gets the positions from the underlying source.</summary>
    /// <returns>A list of the positions.</returns>
    public async Task<List<BaseballPosition>> GetPositions()
    {
      var positions = await _positionRepo.GetPositions();
      return positions
        .Select(p => _mapper.Map<PositionEntity, BaseballPosition>(p))
        .OrderBy(p => p.SortOrder)
        .ToList();
    }
  }
}