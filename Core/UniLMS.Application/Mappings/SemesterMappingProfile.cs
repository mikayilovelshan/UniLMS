using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Semesters;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class SemesterMappingProfile : Profile
    {
        public SemesterMappingProfile()
        {
            CreateMap<CreateSemesterDTO, Semester>();
            CreateMap<UpdateSemesterDTO, Semester>();
            CreateMap<Semester, GetSemesterDTO>();
        }
    }
}
