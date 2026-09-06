using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Specialities;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class SpecialityMappingProfile : Profile
    {
        public SpecialityMappingProfile()
        {
            CreateMap<Speciality, GetSpecialityDTO>()
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty != null ? src.Faculty.Name : null))
                .ForMember(dest => dest.CafedraName, opt => opt.MapFrom(src => src.Cafedra != null ? src.Cafedra.Name : null));

            CreateMap<CreateSpecialityDTO, Speciality>()
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

            CreateMap<UpdateSpecialityDTO, Speciality>()
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

           
        }
    }
}
