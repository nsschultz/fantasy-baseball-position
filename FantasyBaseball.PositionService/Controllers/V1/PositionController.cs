using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyBaseball.PositionService.Controllers.V1
{
    /// <summary>Endpoint for retrieving position data.</summary>
    [Route("api/v1/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IBaseballPositionBuilderService _positionBuilder;
        private readonly IGetPositionsService _getService;
        private readonly ISortService _sortService;

        /// <summary>Creates a new instance of the controller.</summary>
        /// <param name="positionBuilder">Service for converting a PositionEntity to a BaseballPosition.</param>
        /// <param name="getService">Service for getting positions.</param>
        /// <param name="sortService">The service for sorting the positions.</param>
        public PositionController(IBaseballPositionBuilderService positionBuilder,
                                  IGetPositionsService getService,
                                  ISortService sortService) =>
            (_positionBuilder, _getService, _sortService) = (positionBuilder, getService, sortService);

        /// <summary>Gets all of the positions from the source.</summary>
        /// <returns>All of the positions from the source.</returns>
        [HttpGet]
        public async Task<List<BaseballPosition>> GetPositions()
        {
            var positions = await _getService.GetPositions();
            var baseballPositions = positions.Select(position => _positionBuilder.BuildBaseballPosition(position)).ToList();
            return _sortService.SortPositions(baseballPositions);
        }
    }
}