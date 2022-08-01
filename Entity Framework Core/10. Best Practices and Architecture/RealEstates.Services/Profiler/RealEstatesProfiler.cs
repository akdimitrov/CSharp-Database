using System.Linq;
using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services.Profiler
{
    public class RealEstatesProfiler : Profile
    {
        public RealEstatesProfiler()
        {
            CreateMap<Property, PropertyInfoDto>()
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Price ?? 0))
                .ForMember(x => x.PropertyType, y => y.MapFrom(s => s.Type.Name))
                .ForMember(x => x.BuildingType, y => y.MapFrom(s => s.BuildingType.Name));

            CreateMap<District, DistrictInfoDto>()
                .ForMember(x => x.AveragePricePerSquareMeter, y => y.MapFrom(s => s.Properties
                        .Where(p => p.Price.HasValue)
                        .Average(p => p.Price / (decimal)p.Size) ?? 0));

            CreateMap<Tag, TagInfoDto>();

            CreateMap<Property, PropertyInfoFullDataDto>()
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Price ?? 0))
                .ForMember(x => x.PropertyType, y => y.MapFrom(s => s.Type.Name))
                .ForMember(x => x.BuildingType, y => y.MapFrom(s => s.BuildingType.Name))
                .ForMember(x => x.Year, y => y.MapFrom(s => s.Year ?? 0));
        }
    }
}
