using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Database.Entities;

namespace FantasyBaseball.PositionService.Maps;

/// <summary>AutoMapper profile for mapping Position entities to DTOs.</summary>
public class BaseballPositionProfile : Profile
{
    /// <summary>Initializes a new instance of the AutoMapper profile for position mappings.</summary>
    public BaseballPositionProfile() =>
        CreateMap<PositionEntity, Position>()
            .ForMember(dest => dest.AdditionalPositions,
                opt => opt.MapFrom(src => src.ParentPositions.Select(p => p.ChildPosition)))
            .AfterMap((src, dest) => dest.AdditionalPositions.ForEach(ap => ap.AdditionalPositions = []));
}