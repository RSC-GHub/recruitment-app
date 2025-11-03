using AutoMapper;
using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Domain.Entities.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruitment.Application.MappingProfiles
{
    public class CoreBusinessProfile : Profile
    {
        public CoreBusinessProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<CreateCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>();
        }
    }
}
