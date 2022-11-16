using System.Collections.Generic;
using System.Linq;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Entities;
using Xunit;

namespace FantasyBaseball.PositionService.Services.UnitTests
{
    public class BaseballPositionBuilderServiceTest
    {
        private static readonly PositionEntity DB_1B = new PositionEntity
        {
            Code = "1B",
            FullName = "First Baseman",
            PlayerType = PlayerType.B,
            SortOrder = 1
        };
        private static readonly PositionEntity DB_CIF = new PositionEntity
        {
            Code = "CIF",
            FullName = "Corner Infielder",
            PlayerType = PlayerType.B,
            SortOrder = 5
        };
        private static readonly PositionEntity DB_IF = new PositionEntity
        {
            Code = "IF",
            FullName = "Infielder",
            PlayerType = PlayerType.B,
            SortOrder = 7
        };
        private static readonly PositionEntity DB_UTIL = new PositionEntity
        {
            Code = "UTIL",
            FullName = "Utility",
            PlayerType = PlayerType.B,
            SortOrder = 13
        };
        private static List<PositionEntity> DatabasePositions = new List<PositionEntity> { DB_1B, DB_CIF, DB_IF, DB_UTIL };
        private static readonly List<BaseballPosition> ExpectedPositions = new List<BaseballPosition>
        {
            new BaseballPosition
            {
                Code = "1B",
                FullName = "First Baseman",
                PlayerType = PlayerType.B,
                SortOrder = 1,
                AdditionalPositions = new List<BaseballPosition>
                {
                    new BaseballPosition { Code = "CIF", FullName = "Corner Infielder", PlayerType = PlayerType.B, SortOrder = 5 },
                    new BaseballPosition { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 },
                    new BaseballPosition { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
                }
            },
            new BaseballPosition
            {
                Code = "CIF",
                FullName = "Corner Infielder",
                PlayerType = PlayerType.B,
                SortOrder = 5,
                AdditionalPositions = new List<BaseballPosition>
                {
                    new BaseballPosition { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 },
                    new BaseballPosition { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
                }
            },
            new BaseballPosition
            {
                Code = "IF",
                FullName = "Infielder",
                PlayerType = PlayerType.B,
                SortOrder = 7,
                AdditionalPositions = new List<BaseballPosition>
                {
                    new BaseballPosition { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
                }
            },
            new BaseballPosition { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
        };

        [Fact] public void BuildBaseballPositionNullTest() => Assert.Equal(0, new BaseballPositionBuilderService().BuildBaseballPosition(null).SortOrder);

        [Fact]
        public void BuildBaseballPositionValidTest()
        {
            AddRelationship(DB_1B, DB_CIF);
            AddRelationship(DB_1B, DB_IF);
            AddRelationship(DB_1B, DB_UTIL);
            AddRelationship(DB_CIF, DB_IF);
            AddRelationship(DB_CIF, DB_UTIL);
            AddRelationship(DB_IF, DB_UTIL);
            ExpectedPositions.ForEach(expected =>
            {
                var databasePosition = DatabasePositions.First(dp => dp.Code == expected.Code);
                var actual = new BaseballPositionBuilderService().BuildBaseballPosition(databasePosition);
                ValidatePosition(expected, actual);
            });
        }

        private static void AddRelationship(PositionEntity parent, PositionEntity child)
        {
            var relationship = new AdditionalPositionEntity { ParentCode = parent.Code, ChildCode = child.Code, ParentPosition = parent, ChildPosition = child };
            parent.ParentPositions.Add(relationship);
            child.ChildPositions.Add(relationship);
        }

        private static void ValidatePosition(BaseballPosition expected, BaseballPosition actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.PlayerType, actual.PlayerType);
            Assert.Equal(expected.SortOrder, actual.SortOrder);
            Assert.Equal(expected.AdditionalPositions.Count, actual.AdditionalPositions.Count);
            expected.AdditionalPositions.ForEach(e => ValidatePosition(e, actual.AdditionalPositions.First(dp => dp.Code == e.Code)));
        }
    }
}