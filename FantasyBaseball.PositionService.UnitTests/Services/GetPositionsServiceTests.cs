using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Maps;
using FantasyBaseball.PositionService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Services;

/// <summary>Tests for the <see cref="GetPositionsService"/>.</summary>
public class GetPositionsServiceTests
{
  private static readonly List<PositionEntity> TestPositions =
  [
    new() { Code = "C"   , FullName = "Catcher"          , PlayerType = PlayerType.B, SortOrder = 0            },
    new() { Code = "CF"  , FullName = "Center Fielder"   , PlayerType = PlayerType.B, SortOrder = 9            },
    new() { Code = "CIF" , FullName = "Corner Infielder" , PlayerType = PlayerType.B, SortOrder = 5            },
    new() { Code = "DH"  , FullName = "Designated Hitter", PlayerType = PlayerType.B, SortOrder = 12           },
    new() { Code = "1B"  , FullName = "First Baseman"    , PlayerType = PlayerType.B, SortOrder = 1            },
    new() { Code = "IF"  , FullName = "Infielder"        , PlayerType = PlayerType.B, SortOrder = 7            },
    new() { Code = "LF"  , FullName = "Left Fielder"     , PlayerType = PlayerType.B, SortOrder = 8            },
    new() { Code = "MIF" , FullName = "Middle Infielder" , PlayerType = PlayerType.B, SortOrder = 6            },
    new() { Code = "OF"  , FullName = "Outfielder"       , PlayerType = PlayerType.B, SortOrder = 11           },
    new() { Code = "P"   , FullName = "Pitcher"          , PlayerType = PlayerType.P, SortOrder = 102          },
    new() { Code = "RP"  , FullName = "Relief Pitcher"   , PlayerType = PlayerType.P, SortOrder = 101          },
    new() { Code = "RF"  , FullName = "Right Fielder"    , PlayerType = PlayerType.B, SortOrder = 10           },
    new() { Code = "2B"  , FullName = "Second Baseman"   , PlayerType = PlayerType.B, SortOrder = 2            },
    new() { Code = "SS"  , FullName = "Shortstop"        , PlayerType = PlayerType.B, SortOrder = 4            },
    new() { Code = "SP"  , FullName = "Starting Pitcher" , PlayerType = PlayerType.P, SortOrder = 100          },
    new() { Code = "3B"  , FullName = "Third Baseman"    , PlayerType = PlayerType.B, SortOrder = 3            },
    new() { Code = ""    , FullName = "Unknown"          , PlayerType = PlayerType.U, SortOrder = int.MaxValue },
    new() { Code = "UTIL", FullName = "Utility"          , PlayerType = PlayerType.B, SortOrder = 13           }
  ];

  private static readonly List<string> SortedCodes =
  [
    "C", "1B", "2B", "3B", "SS", "CIF", "MIF", "IF", "LF", "CF", "RF", "OF", "DH", "UTIL", "SP", "RP", "P", ""
  ];

  /// <summary>Verifies that GetPositions returns all positions sorted by SortOrder.</summary>
  [Fact]
  public async Task GetPositions_ReturnsSortedPositions()
  {
    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();
    var positionRepo = new Mock<IPositionRepository>();
    positionRepo.Setup(o => o.GetPositions()).ReturnsAsync(TestPositions);
    var positions = await new GetPositionsService(mapper, positionRepo.Object).GetPositions();
    
    Assert.Equal(18, positions.Count);
    for (int x = 0; x < 18; x++)
    {
      ValidatePosition(TestPositions.First(p => p.Code == SortedCodes[x]), positions[x]);
    }
  }

  /// <summary>Verifies that GetPositions returns empty collection when repository returns empty list.</summary>
  [Fact]
  public async Task GetPositions_WithEmptyRepository_ReturnsEmptyList()
  {
    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();
    var positionRepo = new Mock<IPositionRepository>();
    positionRepo.Setup(o => o.GetPositions()).ReturnsAsync(new List<PositionEntity>());
    
    var positions = await new GetPositionsService(mapper, positionRepo.Object).GetPositions();
    
    Assert.Empty(positions);
  }

  /// <summary>Verifies that GetPositions properly sorts when positions have duplicate SortOrder values.</summary>
  [Fact]
  public async Task GetPositions_WithDuplicateSortOrder_SortsCorrectly()
  {
    var testData = new List<PositionEntity>
    {
      new() { Code = "P1", FullName = "Position 1", PlayerType = PlayerType.B, SortOrder = 5 },
      new() { Code = "P2", FullName = "Position 2", PlayerType = PlayerType.B, SortOrder = 3 },
      new() { Code = "P3", FullName = "Position 3", PlayerType = PlayerType.B, SortOrder = 5 }
    };

    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();
    var positionRepo = new Mock<IPositionRepository>();
    positionRepo.Setup(o => o.GetPositions()).ReturnsAsync(testData);
    
    var positions = await new GetPositionsService(mapper, positionRepo.Object).GetPositions();
    
    Assert.Equal(3, positions.Count);
    Assert.Equal(3, positions[0].SortOrder);
    Assert.True(positions[1].SortOrder == 5);
    Assert.True(positions[2].SortOrder == 5);
  }

  /// <summary>Verifies that GetPositions handles negative SortOrder values.</summary>
  [Fact]
  public async Task GetPositions_WithNegativeSortOrder_SortsCorrectly()
  {
    var testData = new List<PositionEntity>
    {
      new() { Code = "P1", FullName = "Position 1", PlayerType = PlayerType.B, SortOrder = 5 },
      new() { Code = "P2", FullName = "Position 2", PlayerType = PlayerType.B, SortOrder = -1 }
    };

    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();
    var positionRepo = new Mock<IPositionRepository>();
    positionRepo.Setup(o => o.GetPositions()).ReturnsAsync(testData);
    
    var positions = await new GetPositionsService(mapper, positionRepo.Object).GetPositions();
    
    Assert.Equal(2, positions.Count);
    Assert.Equal(-1, positions[0].SortOrder);
    Assert.Equal(5, positions[1].SortOrder);
  }

  /// <summary>Verifies that repository exceptions are propagated.</summary>
  [Fact]
  public async Task GetPositions_WhenRepositoryThrows_PropagatesException()
  {
    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();
    var positionRepo = new Mock<IPositionRepository>();
    positionRepo.Setup(o => o.GetPositions()).ThrowsAsync(new InvalidOperationException("Database error"));
    
    var service = new GetPositionsService(mapper, positionRepo.Object);
    
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetPositions());
  }

  private static void ValidatePosition(PositionEntity expected, Position actual)
  {
    Assert.Equal(expected.Code, actual.Code);
    Assert.Equal(expected.FullName, actual.FullName);
    Assert.Equal(expected.PlayerType, actual.PlayerType);
    Assert.Equal(expected.SortOrder, actual.SortOrder);
  }
}
