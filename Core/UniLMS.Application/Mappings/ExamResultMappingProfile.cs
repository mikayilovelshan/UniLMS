using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class ExamResultMappingProfile : Profile 
    {
        public ExamResultMappingProfile()
        {
            CreateMap<CreateExamResultRangeDTO, ExamResult>();

            CreateMap<CreateStudentExamResultDTO, ExamResult>();

            CreateMap<UpdateExamResultDTO, ExamResult>();

            CreateMap<ExamResult, GetExamResultDTO>()
                .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.Student.AppUser.FirstName + " " + src.Student.AppUser.LastName))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseOffering.Course.Name))
                .ForMember(dest => dest.ExamTypeName, opt => opt.MapFrom(src => src.ExamType.ToString()));
        }
    }
}
