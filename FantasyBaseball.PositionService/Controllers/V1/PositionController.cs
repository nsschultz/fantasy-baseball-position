using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Models;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyBaseball.PositionService.Controllers.V1
{
  /// <summary>Endpoint for retrieving position data.</summary>
  /// <remarks>Creates a new instance of the controller.</remarks>
  /// <param name="getService">Service for getting positions.</param>
  [Route("api/v1/position")]
  [ApiController]
  public class PositionController(IGetPositionsService getService) : ControllerBase
  {
    private readonly IGetPositionsService _getService = getService;

    /// <summary>Gets all of the positions from the source.</summary>
    /// <returns>All of the positions from the source.</returns>
    [HttpGet]
    public async Task<List<BaseballPosition>> GetPositions() => await _getService.GetPositions();
  }
}