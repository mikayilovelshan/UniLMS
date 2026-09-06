using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.Mappings
{
    public class CafedraMappingProfile : Profile
    {
        public CafedraMappingProfile()
        {
            CreateMap<CreateCafedraDTO, Cafedra>();

            CreateMap<UpdateCafedraDTO, Cafedra>();

            CreateMap<Cafedra, GetCafedraDTO>()
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));
        }
    }
}
