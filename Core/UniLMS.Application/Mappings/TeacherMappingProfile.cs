using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Teachers;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile() 
        {
            CreateMap<Teacher, GetTeacherDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.CafedraName, opt => opt.MapFrom(src => src.Cafedra.Name));

            CreateMap<CreateTeacherDTO, Teacher>();
            CreateMap<UpdateTeacherDTO, Teacher>();
        }
    }
}
