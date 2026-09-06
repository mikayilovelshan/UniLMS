using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Faculties;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class FacultyMappingProfile : Profile
    {
        public FacultyMappingProfile()
        {
            CreateMap<CreateFacultyDTO, Faculty>();

            CreateMap<UpdateFacultyDTO, Faculty>();

            CreateMap<Faculty, GetFacultyDTO>();

        }
    }
}
