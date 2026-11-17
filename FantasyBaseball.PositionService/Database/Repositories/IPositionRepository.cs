using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database.Entities;

namespace FantasyBaseball.PositionService.Database.Repositories;

/// <summary>Repository for CRUD functionality for positions.</summary>
public interface IPositionRepository
{
    /// <summary>Gets all positions from the database with their relationships loaded.</summary>
    /// <returns>A list of all positions including child and parent position relationships.</returns>
    Task<List<PositionEntity>> GetPositions();
}