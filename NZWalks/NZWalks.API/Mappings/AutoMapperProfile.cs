using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>();
            CreateMap<UpdateRegionRequestDto, Region>();
            CreateMap<AddWalkRequestDto, Walk>();
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<DifficultyDto, Difficulty>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }
    }
}
