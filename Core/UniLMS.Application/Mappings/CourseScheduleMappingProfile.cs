using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseSchedules;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class CourseScheduleMappingProfile : Profile
    {
        public CourseScheduleMappingProfile() 
        {
            CreateMap<CreateCourseScheduleDTO, CourseSchedule>();

            CreateMap<UpdateCourseScheduleDTO, CourseSchedule>();

            CreateMap<CourseSchedule, GetCourseScheduleDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseOffering.Course.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.CourseOffering.Teacher.AppUser.FirstName} {src.CourseOffering.Teacher.AppUser.LastName}"))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.CourseOffering.Group.Code))
                .ForMember(dest => dest.RoomCode, opt => opt.MapFrom(src => src.CourseOffering.RoomCode));


        }
    }
}
