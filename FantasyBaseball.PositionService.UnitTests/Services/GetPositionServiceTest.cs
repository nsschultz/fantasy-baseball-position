using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Maps;
using FantasyBaseball.PositionService.Models;
using FantasyBaseball.PositionService.Models.Enums;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.Services.UnitTests
{
  public class GetPositionServiceTest
  {
    private static readonly List<PositionEntity> POSITIONS =
    [
      new() { Code = "C"   , FullName = "Catcher"          , PlayerType = PlayerType.B, SortOrder = 0            },
      new() { Code = "CF"  , FullName = "Center Feilder"   , PlayerType = PlayerType.B, SortOrder = 9            },
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
    private static readonly List<string> SORTED_CODES =
    [
      "C", "1B", "2B", "3B", "SS", "CIF", "MIF", "IF", "LF", "CF", "RF", "OF", "DH", "UTIL", "SP", "RP", "P", ""
    ];

    [Fact]
    public async void GetPositionsTest()
    {
      var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile())).CreateMapper();
      var positionRepo = new Mock<IPositionRepository>();
      positionRepo.Setup(o => o.GetPositions()).ReturnsAsync(POSITIONS);
      var positions = await new GetPositionsService(mapper, positionRepo.Object).GetPositions();
      Assert.Equal(18, positions.Count);
      for (int x = 0; x < 18; x++) ValidatePosition(POSITIONS.First(p => p.Code == SORTED_CODES[x]), positions[x]);
    }

    private static void ValidatePosition(PositionEntity expected, BaseballPosition actual)
    {
      Assert.Equal(expected.Code, actual.Code);
      Assert.Equal(expected.FullName, actual.FullName);
      Assert.Equal(expected.PlayerType, actual.PlayerType);
      Assert.Equal(expected.SortOrder, actual.SortOrder);
    }
  }
}
