using AutoMapper;
using BusinessLayer.Model.Models;
using System;
using System.Globalization;
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
            CreateMap<CompanyInfo, CompanyDto>();
            CreateMap<ArSubledgerInfo, ArSubledgerDto>();
            CreateMap<EmployeeInfo, EmployeeDto>()
                .ForMember(e => e.PhoneNumber, member => member.MapFrom(src => src.Phone))
                .ForMember(e => e.OccupationName, member => member.MapFrom(src => src.Occupation))
                .ForMember(e => e.LastModifiedDateTime, member => member.MapFrom(src => ((DateTime)src.LastModified).ToShortDateString()));
            CreateMap<EmployeeDto, EmployeeInfo>()
                .ForMember(e => e.Phone, member => member.MapFrom(src => src.PhoneNumber))
                .ForMember(e => e.Occupation, member => member.MapFrom(src => src.OccupationName))
                .ForMember(e => e.LastModified, member => member.MapFrom(src => (DateTime.Parse(src.LastModifiedDateTime))));
        }
    }
}