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
        private static List<PositionEntity> DatabasePositions = new List<PositionEntity>
            {
                new PositionEntity
                {
                    Code = "SP",
                    FullName = "Starting Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 100,
                    ParentPositions = new List<AdditionalPositionEntity>
                    {
                        new AdditionalPositionEntity
                        {
                            ParentCode = "SP",
                            ChildCode = "P",
                            ParentPosition = new PositionEntity { Code = "SP", FullName = "Starting Pitcher", PlayerType = PlayerType.P, SortOrder = 100 },
                            ChildPosition = new PositionEntity { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                        }
                    }
                },
                new PositionEntity
                {
                    Code = "RP",
                    FullName = "Relief Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 101,
                    ParentPositions = new List<AdditionalPositionEntity>
                    {
                        new AdditionalPositionEntity
                        {
                            ParentCode = "RP",
                            ChildCode = "P",
                            ParentPosition = new PositionEntity { Code = "RP", FullName = "Relief Pitcher", PlayerType = PlayerType.P, SortOrder = 101 },
                            ChildPosition = new PositionEntity { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                        }
                    }
                },
                new PositionEntity
                {
                    Code = "P",
                    FullName = "Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 102,
                    ChildPositions = new List<AdditionalPositionEntity>
                    {
                        new AdditionalPositionEntity
                        {
                            ParentCode = "SP",
                            ChildCode = "P",
                            ParentPosition = new PositionEntity { Code = "SP", FullName = "Starting Pitcher", PlayerType = PlayerType.P, SortOrder = 100 },
                            ChildPosition = new PositionEntity { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                        },
                        new AdditionalPositionEntity
                        {
                            ParentCode = "RP",
                            ChildCode = "P",
                            ParentPosition = new PositionEntity { Code = "RP", FullName = "Relief Pitcher", PlayerType = PlayerType.P, SortOrder = 101 },
                            ChildPosition = new PositionEntity { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                        }
                    }
                }
            };

        private static List<BaseballPosition> ExpectedPositions = new List<BaseballPosition>
            {
                new BaseballPosition
                {
                    Code = "SP",
                    FullName = "Starting Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 100,
                    AddtionalPositions = new List<BaseballPosition>
                    {
                        new BaseballPosition { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                    }
                },
                new BaseballPosition
                {
                    Code = "RP",
                    FullName = "Relief Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 101,
                    AddtionalPositions = new List<BaseballPosition>
                    {
                        new BaseballPosition { Code = "P", FullName = "Pitcher", PlayerType = PlayerType.P, SortOrder = 102 }
                    }
                },
                new BaseballPosition
                {
                    Code = "P",
                    FullName = "Pitcher",
                    PlayerType = PlayerType.P,
                    SortOrder = 102
                }
            };

        [Fact] public void BuildBaseballPositionNullTest() => Assert.Equal(0, new BaseballPositionBuilderService().BuildBaseballPosition(null).SortOrder);

        [Fact]
        public void BuildBaseballPositionValidTest()
        {
            ExpectedPositions.ForEach(expected =>
                {
                    var databasePosition = DatabasePositions.First(dp => dp.Code == expected.Code);
                    var actual = new BaseballPositionBuilderService().BuildBaseballPosition(databasePosition);
                    ValidatePosition(expected, actual);
                }
            );
        }

        private static void ValidatePosition(BaseballPosition expected, BaseballPosition actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.PlayerType, actual.PlayerType);
            Assert.Equal(expected.SortOrder, actual.SortOrder);
            if (expected.AddtionalPositions.Count > 0) ValidatePosition(expected.AddtionalPositions.First(), actual.AddtionalPositions.First());
        }
    }
}