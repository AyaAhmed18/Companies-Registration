using AutoMapper;
using Data;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignUpDto, Company>().ReverseMap();
            CreateMap<SetPasswordDto, Company>().ReverseMap();
            CreateMap<LoginDto, Company>().ReverseMap();
            CreateMap<GetCompanyDataDto,Company>().ReverseMap();
            
        }
    }
}
