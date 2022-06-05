using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace FantasyBaseball.PositionService.Services.UnitTests
{
    public class GetPositionServiceTest : IDisposable
    {
        private static Dictionary<string, PositionEntity> ExpectedCollection = new List<PositionEntity>
        {
            new PositionEntity { Code = ""    , FullName = "Unknown"          , PlayerType = PlayerType.U, SortOrder = int.MaxValue },
            new PositionEntity { Code = "C"   , FullName = "Catcher"          , PlayerType = PlayerType.B, SortOrder = 0            },
            new PositionEntity { Code = "1B"  , FullName = "First Baseman"    , PlayerType = PlayerType.B, SortOrder = 1            },
            new PositionEntity { Code = "2B"  , FullName = "Second Baseman"   , PlayerType = PlayerType.B, SortOrder = 2            },
            new PositionEntity { Code = "3B"  , FullName = "Third Baseman"    , PlayerType = PlayerType.B, SortOrder = 3            },
            new PositionEntity { Code = "SS"  , FullName = "Shortstop"        , PlayerType = PlayerType.B, SortOrder = 4            },
            new PositionEntity { Code = "CIF" , FullName = "Corner Infielder" , PlayerType = PlayerType.B, SortOrder = 5            },
            new PositionEntity { Code = "MIF" , FullName = "Middle Infielder" , PlayerType = PlayerType.B, SortOrder = 6            },
            new PositionEntity { Code = "IF"  , FullName = "Infielder"        , PlayerType = PlayerType.B, SortOrder = 7            },
            new PositionEntity { Code = "LF"  , FullName = "Left Fielder"     , PlayerType = PlayerType.B, SortOrder = 8            },
            new PositionEntity { Code = "CF"  , FullName = "Center Feilder"   , PlayerType = PlayerType.B, SortOrder = 9            },
            new PositionEntity { Code = "RF"  , FullName = "Right Fielder"    , PlayerType = PlayerType.B, SortOrder = 10           },
            new PositionEntity { Code = "OF"  , FullName = "Outfielder"       , PlayerType = PlayerType.B, SortOrder = 11           },
            new PositionEntity { Code = "DH"  , FullName = "Designated Hitter", PlayerType = PlayerType.B, SortOrder = 12           },
            new PositionEntity { Code = "UTIL", FullName = "Utility"          , PlayerType = PlayerType.B, SortOrder = 13           },
            new PositionEntity { Code = "SP"  , FullName = "Starting Pitcher" , PlayerType = PlayerType.P, SortOrder = 100          },
            new PositionEntity { Code = "RP"  , FullName = "Relief Pitcher"   , PlayerType = PlayerType.P, SortOrder = 101          },
            new PositionEntity { Code = "P"   , FullName = "Pitcher"          , PlayerType = PlayerType.P, SortOrder = 102          }
        }.ToDictionary(p => p.Code, p => p);

        private static Dictionary<string, int> ExpectedChildCount = new Dictionary<string, string[]>
        {
            { ""    , new string [] { } },
            { "C"   , new string [] { } },
            { "1B"  , new string [] { } },
            { "2B"  , new string [] { } },
            { "3B"  , new string [] { } },
            { "SS"  , new string [] { } },
            { "CIF" , new [] { "1B", "3B" } },
            { "MIF" , new [] { "2B", "3B" } },
            { "IF"  , new [] { "1B", "2B", "3B", "SS", "CIF", "MIF" } },
            { "LF"  , new string [] { } },
            { "CF"  , new string [] { } },
            { "RF"  , new string [] { } },
            { "OF"  , new [] { "LF", "CF", "RF" } },
            { "DH"  , new string [] { } },
            { "UTIL", new [] {"C", "1B", "2B", "3B", "SS", "CIF", "MIF", "IF", "LF", "CF", "RF", "OF", "DH" } },
            { "SP"  , new string [] { } },
            { "RP"  , new string [] { } },
            { "P"   , new [] { "SP", "RP" } }
        }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count());

        private static Dictionary<string, int> ExpectedParentCount = new Dictionary<string, string[]>
        {
            { ""    , new string [] { } },
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
            { "UTIL", new string [] { } },
            { "SP"  , new [] { "P" } },
            { "RP"  , new [] { "P" } },
            { "P"   , new string [] { } }
        }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count());

        private PositionContext _context;

        public GetPositionServiceTest() => _context = CreateContext().Result;

        [Fact] public async void GetPositionsTest()
        {
            var positions = await new GetPositionsService(_context).GetPositions();
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
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private async Task<PositionContext> CreateContext()
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
}