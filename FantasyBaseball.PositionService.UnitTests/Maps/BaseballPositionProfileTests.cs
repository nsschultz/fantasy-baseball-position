using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyBaseball.Common.Enums;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Maps;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FantasyBaseball.PositionService.UnitTests.Maps;

/// <summary>Tests for the <see cref="BaseballPositionProfile"/> AutoMapper profile.</summary>
public class BaseballPositionProfileTests
{
    private static readonly PositionEntity FirstBaseman = new() { Code = "1B", FullName = "First Baseman", PlayerType = PlayerType.B, SortOrder = 1 };
    private static readonly PositionEntity CornerInfielder = new() { Code = "CIF", FullName = "Corner Infielder", PlayerType = PlayerType.B, SortOrder = 5 };
    private static readonly PositionEntity Infielder = new() { Code = "IF", FullName = "Infielder", PlayerType = PlayerType.B, SortOrder = 7 };
    private static readonly PositionEntity Utility = new() { Code = "UTIL", FullName = "Utility", PlayerType = PlayerType.B, SortOrder = 13 };

    private static readonly List<PositionEntity> DatabasePositions = [FirstBaseman, CornerInfielder, Infielder, Utility];

    private static readonly List<Position> ExpectedMappings =
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

    public BaseballPositionProfileTests() => _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new BaseballPositionProfile()), new LoggerFactory()).CreateMapper();

    /// <summary>Verifies that mapping a null PositionEntity returns null.</summary>
    [Fact]
    public void MapNull_ReturnsNull()
    {
        var result = _mapper.Map<Position>(null);
        Assert.Null(result);
    }

    /// <summary>Verifies that PositionEntity instances are correctly mapped to Position objects with relationships.</summary>
    [Fact]
    public void MapPosition_MapsCorrectlyWithRelationships()
    {
        SetupRelationships(FirstBaseman, CornerInfielder);
        SetupRelationships(FirstBaseman, Infielder);
        SetupRelationships(FirstBaseman, Utility);
        SetupRelationships(CornerInfielder, Infielder);
        SetupRelationships(CornerInfielder, Utility);
        SetupRelationships(Infielder, Utility);

        foreach (var expected in ExpectedMappings)
        {
            var source = DatabasePositions.First(p => p.Code == expected.Code);
            var actual = _mapper.Map<Position>(source);
            ValidateMapping(expected, actual);
        }
    }

    private static void SetupRelationships(PositionEntity parent, PositionEntity child)
    {
        var relationship = new AdditionalPositionEntity
        {
            ParentCode = parent.Code,
            ChildCode = child.Code,
            ParentPosition = parent,
            ChildPosition = child
        };
        parent.ParentPositions.Add(relationship);
        child.ChildPositions.Add(relationship);
    }

    private static void ValidateMapping(Position expected, Position actual)
    {
        Assert.Equal(expected.Code, actual.Code);
        Assert.Equal(expected.FullName, actual.FullName);
        Assert.Equal(expected.PlayerType, actual.PlayerType);
        Assert.Equal(expected.SortOrder, actual.SortOrder);
        Assert.Equal(expected.AdditionalPositions.Count, actual.AdditionalPositions.Count);

        foreach (var expectedAdditional in expected.AdditionalPositions)
        {
            ValidateMapping(expectedAdditional, actual.AdditionalPositions.First(p => p.Code == expectedAdditional.Code));
        }
    }
}