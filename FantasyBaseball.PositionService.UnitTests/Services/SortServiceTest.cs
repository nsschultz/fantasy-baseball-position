using System.Collections.Generic;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.Common.Models;
using Xunit;

namespace FantasyBaseball.PositionService.Services.UnitTests
{
    public class SortServiceTest
    {
        [Fact] public void SortTest()
        {
            var positionList = new List<BaseballPosition>
            {
                BuildPosition("C", "Catcher", PlayerType.B, 0),
                BuildPosition("CF", "Center Feilder", PlayerType.B, 9),
                BuildPosition("CIF", "Corner Infielder", PlayerType.B, 5),
                BuildPosition("DH", "Designated Hitter", PlayerType.B, 12),
                BuildPosition("1B", "First Baseman", PlayerType.B, 1),
                BuildPosition("IF", "Infielder", PlayerType.B, 7),
                BuildPosition("LF", "Left Fielder", PlayerType.B, 8),
                BuildPosition("MIF", "Middle Infielder", PlayerType.B, 6),
                BuildPosition("OF", "Outfielder", PlayerType.B, 11),
                BuildPosition("P", "Pitcher", PlayerType.P, 102),
                BuildPosition("RP", "Relief Pitcher", PlayerType.P, 101),
                BuildPosition("RF", "Right Fielder", PlayerType.B, 10),
                BuildPosition("2B", "Second Baseman", PlayerType.B, 2),
                BuildPosition("SS", "Shortstop", PlayerType.B, 4),
                BuildPosition("SP", "Starting Pitcher", PlayerType.P, 100),
                BuildPosition("3B", "Third Baseman", PlayerType.B, 3),
                BuildPosition("", "Unknown", PlayerType.U, int.MaxValue),
                BuildPosition("UTIL", "Utility", PlayerType.B, 13)
            };
            var sortedList = new SortService().SortPositions(positionList);
            Assert.Equal("C"   , sortedList[0].Code );
            Assert.Equal("1B"  , sortedList[1].Code );
            Assert.Equal("2B"  , sortedList[2].Code );
            Assert.Equal("3B"  , sortedList[3].Code );
            Assert.Equal("SS"  , sortedList[4].Code );
            Assert.Equal("CIF" , sortedList[5].Code );
            Assert.Equal("MIF" , sortedList[6].Code );
            Assert.Equal("IF"  , sortedList[7].Code );
            Assert.Equal("LF"  , sortedList[8].Code );
            Assert.Equal("CF"  , sortedList[9].Code );
            Assert.Equal("RF"  , sortedList[10].Code);
            Assert.Equal("OF"  , sortedList[11].Code);
            Assert.Equal("DH"  , sortedList[12].Code);
            Assert.Equal("UTIL", sortedList[13].Code);
            Assert.Equal("SP"  , sortedList[14].Code);
            Assert.Equal("RP"  , sortedList[15].Code);
            Assert.Equal("P"   , sortedList[16].Code);
            Assert.Equal(""    , sortedList[17].Code);
        }

        private static BaseballPosition BuildPosition(string code, string fullname, PlayerType type, int sortOrder) => 
            new BaseballPosition { Code = code, FullName = fullname, PlayerType = type, SortOrder = sortOrder };
    }
}