using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile() 
        {
            CreateMap<CreateStudentAttendanceDTO, Attendance>();

            CreateMap<UpdateStudentAttendanceDTO, Attendance>();

            CreateMap<Attendance, GetStudentAttendanceDTO>()
                .ForMember(dest => dest.AttendanceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.Student.AppUser.FirstName + " " + src.Student.AppUser.LastName));

            CreateMap<CourseSchedule, GetAttendanceDTO>()
                .ForMember(dest => dest.CourseScheduleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseOffering.Course.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.CourseOffering.Group.Code))
                .ForMember(dest => dest.Students, opt => opt.Ignore());
        }
    }
}
