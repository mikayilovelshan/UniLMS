using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Students;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, GetStudentDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.Group != null ? src.Group.Code : null));

            CreateMap<CreateStudentDTO, Student>();
            CreateMap<UpdateStudentDTO, Student>();
        }
    }
}

