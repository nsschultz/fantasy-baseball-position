using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Maps;
using FantasyBaseball.PositionService.Models;
using FantasyBaseball.PositionService.Models.Enums;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Maps
{
  public class BaseballPositionProfileTest
  {
    private static readonly PositionEntity DB_1B = new() { Code = "1B", FullName = "First Baseman", PlayerType = PlayerType.B, SortOrder = 1 };
    private static readonly PositionEntity DB_CIF = new() { Code = "CIF", FullName = "Corner Infielder", PlayerType = PlayerType.B, SortOrder = 5 };
    private static readonly PositionEntity DB_IF = new() { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 };
    private static readonly PositionEntity DB_UTIL = new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 };
    private static readonly List<PositionEntity> DatabasePositions = [DB_1B, DB_CIF, DB_IF, DB_UTIL];
    private static readonly List<BaseballPosition> ExpectedPositions =
    [
      new() {
        Code = "1B",
        FullName = "First Baseman",
        PlayerType = PlayerType.B,
        SortOrder = 1,
        AdditionalPositions =
        [
          new() { Code = "CIF", FullName = "Corner Infielder", PlayerType = PlayerType.B, SortOrder = 5 },
          new() { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 },
          new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
        ]
      },
      new()
      {
        Code = "CIF",
        FullName = "Corner Infielder",
        PlayerType = PlayerType.B,
        SortOrder = 5,
        AdditionalPositions =
        [
          new() { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 },
          new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
        ]
      },
      new()
      {
        Code = "IF",
        FullName = "Infielder",
        PlayerType = PlayerType.B,
        SortOrder = 7,
        AdditionalPositions =
        [
          new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
        ]
      },
      new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 }
    ];
    private readonly IMapper _mapper;

    public BaseballPositionProfileTest() => _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile())).CreateMapper();

    [Fact] public void BuildBaseballPositionNullTest() => Assert.Null(_mapper.Map<BaseballPosition>(null));

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
        var actual = _mapper.Map<BaseballPosition>(databasePosition);
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