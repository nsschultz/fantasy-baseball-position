using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyBaseball.PositionService.Controllers.V1;

/// <summary>Endpoint for retrieving fantasy baseball position data.</summary>
/// <remarks>Provides read-only access to all position types including batters and pitchers.</remarks>
/// <param name="getService">Service for retrieving positions from the database.</param>
[Route("api/v1/position")]
[ApiController]
public class PositionController(IGetPositionsService getService) : ControllerBase
{
    /// <summary>Gets all positions from the data source.</summary>
    /// <returns>A list of all available positions, sorted by sort order.</returns>
    [HttpGet]
    public async Task<List<Position>> GetPositions() => await getService.GetPositions();
}