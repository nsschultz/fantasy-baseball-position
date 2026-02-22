using System.Linq;
using AutoMapper;
using FantasyBaseball.PositionService.Database.Entities;
using FantasyBaseball.PositionService.Models;

namespace FantasyBaseball.PositionService.Maps;

/// <summary>A new profile for the BaseballPosition objects.</summary>
public class BaseballPositionProfile : Profile
{
  /// <summary>Create a new instance of the profile.</summary>
  public BaseballPositionProfile() =>
    CreateMap<PositionEntity, BaseballPosition>()
      .ForMember(dest => dest.AdditionalPositions, opt => opt.MapFrom(src => src.ParentPositions.Select(p => p.ChildPosition)))
      .AfterMap((src, dest) => dest.AdditionalPositions.ForEach(ap => ap.AdditionalPositions = []));
}