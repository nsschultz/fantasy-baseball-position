using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.PositionService.Services
{
    /// <summary>Service for getting positions.</summary>
    public class GetPositionsService : IGetPositionsService
    {
        private readonly IPositionContext _context;

        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="context">The position context.</param>
        public GetPositionsService(IPositionContext context) => _context = context;

        /// <summary>Gets the positions from the underlying source.</summary>
        /// <returns>A list of the positions.</returns>
        public async Task<List<PositionEntity>> GetPositions() =>
            await _context.Positions
                .Include(p => p.ChildPositions)
                .Include(p => p.ParentPositions)
                .ToListAsync();
    }
}