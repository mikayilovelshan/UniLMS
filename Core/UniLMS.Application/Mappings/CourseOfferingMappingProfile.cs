using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseOfferings;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class CourseOfferingMappingProfile :Profile
    {
        public CourseOfferingMappingProfile() 
        {
            CreateMap<CreateCourseOfferingDTO, CourseOffering>();

            CreateMap<UpdateCourseOfferingDTO, CourseOffering>();

            CreateMap<CourseOffering, GetCourseOfferingDTO>()
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.Name))
                .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.Group.Code))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.AppUser.FirstName} {src.Teacher.AppUser.LastName}"))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));
        }
    }
}
