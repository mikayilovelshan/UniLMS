using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Courses;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<UpdateCourseDTO, Course>();
            CreateMap<Course, GetCourseDTO>()
                .ForMember(dest => dest.CafedraName, opt => opt.MapFrom(src => src.Cafedra.Name));
                
        }
    }
}
