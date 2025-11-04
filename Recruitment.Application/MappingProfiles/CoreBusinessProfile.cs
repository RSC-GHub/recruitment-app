using AutoMapper;
using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Application.DTOs.CoreBusiness.Location;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.MappingProfiles
{
    public class CoreBusinessProfile : Profile
    {
        public CoreBusinessProfile()
        {
            // Country
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<CreateCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>();

            // Location
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.CountryName,
                           opt => opt.MapFrom(src => src.Country!.Name))
                .ReverseMap();

            CreateMap<CreateLocationDto, Location>();
            CreateMap<UpdateLocationDto, Location>();

        }
    }
}
