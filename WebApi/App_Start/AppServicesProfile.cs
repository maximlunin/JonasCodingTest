using AutoMapper;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi
{
    public class AppServicesProfile : Profile
    {
        public AppServicesProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<BaseInfo, BaseDto>();
            CreateMap<EmployeeBaseInfo, EmployeeBaseDto>();
            CreateMap<CompanyInfo, CompanyDto>();
            CreateMap<ArSubledgerInfo, ArSubledgerDto>();
            CreateMap<EmployeeInfo, EmployeeDto>()
                .ForMember(dest => dest.OccupationName, opt => opt.MapFrom(src => src.Occupation))
                .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => src.LastModified))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
                //.ForMember(dest => dest.CompanyName, opt => opt.Ignore());
            CreateMap<EmployeeDto, EmployeeInfo>()
                //.ForSourceMember(opt => opt.CompanyName, dest => dest.Ignore())
                .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.OccupationName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.LastModifiedDateTime));


        }
    }
}