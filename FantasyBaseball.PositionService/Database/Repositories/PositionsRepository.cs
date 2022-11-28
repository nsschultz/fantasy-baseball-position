using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyBaseball.PositionService.Database.Repositories
{
    /// <summary>Service for getting positions.</summary>
    public class PositionRepository : IPositionRepository
    {
        private readonly IPositionContext _context;

        /// <summary>Creates a new instance of the repository.</summary>
        /// <param name="context">The position context.</param>
        public PositionRepository(IPositionContext context) => _context = context;

        /// <summary>Gets all of the positions in the database.</summary>
        /// <returns>All of the positions in the database.</returns>
        public async Task<List<PositionEntity>> GetPositions() =>
            await _context.Positions
                .Include(p => p.ChildPositions)
                .Include(p => p.ParentPositions)
                .ToListAsync();
    }
}