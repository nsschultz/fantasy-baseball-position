using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Database.Repositories;

public class PositionRepositoryTest : IDisposable
{
  private static readonly Dictionary<string, PositionEntity> ExpectedCollection = new List<PositionEntity>
  {
    new() { Code = ""    , FullName = "Unknown"          , PlayerType = PlayerType.U, SortOrder = int.MaxValue },
    new() { Code = "C"   , FullName = "Catcher"          , PlayerType = PlayerType.B, SortOrder = 0            },
    new() { Code = "1B"  , FullName = "First Baseman"    , PlayerType = PlayerType.B, SortOrder = 1            },
    new() { Code = "2B"  , FullName = "Second Baseman"   , PlayerType = PlayerType.B, SortOrder = 2            },
    new() { Code = "3B"  , FullName = "Third Baseman"    , PlayerType = PlayerType.B, SortOrder = 3            },
    new() { Code = "SS"  , FullName = "Shortstop"        , PlayerType = PlayerType.B, SortOrder = 4            },
    new() { Code = "CIF" , FullName = "Corner Infielder" , PlayerType = PlayerType.B, SortOrder = 5            },
    new() { Code = "MIF" , FullName = "Middle Infielder" , PlayerType = PlayerType.B, SortOrder = 6            },
    new() { Code = "IF"  , FullName = "Infielder"        , PlayerType = PlayerType.B, SortOrder = 7            },
    new() { Code = "LF"  , FullName = "Left Fielder"     , PlayerType = PlayerType.B, SortOrder = 8            },
    new() { Code = "CF"  , FullName = "Center Feilder"   , PlayerType = PlayerType.B, SortOrder = 9            },
    new() { Code = "RF"  , FullName = "Right Fielder"    , PlayerType = PlayerType.B, SortOrder = 10           },
    new() { Code = "OF"  , FullName = "Outfielder"       , PlayerType = PlayerType.B, SortOrder = 11           },
    new() { Code = "DH"  , FullName = "Designated Hitter", PlayerType = PlayerType.B, SortOrder = 12           },
    new() { Code = "UTIL", FullName = "Utility"          , PlayerType = PlayerType.B, SortOrder = 13           },
    new() { Code = "SP"  , FullName = "Starting Pitcher" , PlayerType = PlayerType.P, SortOrder = 100          },
    new() { Code = "RP"  , FullName = "Relief Pitcher"   , PlayerType = PlayerType.P, SortOrder = 101          },
    new() { Code = "P"   , FullName = "Pitcher"          , PlayerType = PlayerType.P, SortOrder = 102          }
  }.ToDictionary(p => p.Code, p => p);

  private static readonly Dictionary<string, int> ExpectedChildCount = new Dictionary<string, string[]>
  {
    { ""    , Array.Empty<string>() },
    { "C"   , Array.Empty<string>() },
    { "1B"  , Array.Empty<string>() },
    { "2B"  , Array.Empty<string>() },
    { "3B"  , Array.Empty<string>() },
    { "SS"  , Array.Empty<string>() },
    { "CIF" , new [] { "1B", "3B" } },
    { "MIF" , new [] { "2B", "3B" } },
    { "IF"  , new [] { "1B", "2B", "3B", "SS", "CIF", "MIF" } },
    { "LF"  , Array.Empty<string>() },
    { "CF"  , Array.Empty<string>() },
    { "RF"  , Array.Empty<string>() },
    { "OF"  , new [] { "LF", "CF", "RF" } },
    { "DH"  , Array.Empty<string>() },
    { "UTIL", new [] {"C", "1B", "2B", "3B", "SS", "CIF", "MIF", "IF", "LF", "CF", "RF", "OF", "DH" } },
    { "SP"  , Array.Empty<string>() },
    { "RP"  , Array.Empty<string>() },
    { "P"   , new [] { "SP", "RP" } }
  }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Length);

  private static Dictionary<string, int> ExpectedParentCount = new Dictionary<string, string[]>
  {
    { ""    , Array.Empty<string>() },
    { "C"   , new [] { "UTIL" } },
    { "1B"  , new [] { "CIF", "IF", "UTIL" } },
    { "2B"  , new [] { "MIF", "IF", "UTIL" } },
    { "3B"  , new [] { "CIF", "IF", "UTIL" } },
    { "SS"  , new [] { "MIF", "IF", "UTIL" } },
    { "CIF" , new [] { "IF", "UTIL" } },
    { "MIF" , new [] { "IF", "UTIL" } },
    { "IF"  , new [] { "UTIL" } },
    { "LF"  , new [] { "OF", "UTIL" } },
    { "CF"  , new [] { "OF", "UTIL" } },
    { "RF"  , new [] { "OF", "UTIL" } },
    { "OF"  , new [] { "UTIL" } },
    { "DH"  , new [] { "UTIL" } },
    { "UTIL", Array.Empty<string>() },
    { "SP"  , new [] { "P" } },
    { "RP"  , new [] { "P" } },
    { "P"   , Array.Empty<string>() }
  }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Length);

  private readonly PositionContext _context;

  public PositionRepositoryTest() => _context = CreateContext().Result;

  [Fact]
  public async Task GetPositionsTest()
  {
    var positions = await new PositionRepository(_context).GetPositions();
    Assert.Equal(18, positions.Count);
    positions.ForEach(position =>
    {
      var expected = ExpectedCollection[position.Code];
      Assert.Equal(expected.Code, position.Code);
      Assert.Equal(expected.FullName, position.FullName);
      Assert.Equal(expected.PlayerType, position.PlayerType);
      Assert.Equal(expected.SortOrder, position.SortOrder);
      Assert.Equal(ExpectedChildCount[position.Code], position.ChildPositions.Count);
      Assert.Equal(ExpectedParentCount[position.Code], position.ParentPositions.Count);
    });
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing) return;
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

  private static async Task<PositionContext> CreateContext()
  {
    var options = new DbContextOptionsBuilder<PositionContext>()
      .UseInMemoryDatabase(databaseName: "GetPositionsTest")
      .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
      .Options;
    var context = new PositionContext(options);
    context.Database.EnsureCreated();
    Assert.Equal(18, await context.Positions.CountAsync());
    Assert.Equal(28, await context.AdditionalPositions.CountAsync());
    return context;
  }
}