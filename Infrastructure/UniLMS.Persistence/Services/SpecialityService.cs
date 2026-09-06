using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.SpecialityRepository;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Persistence.Services
{
    public class SpecialityService(ISpecialityReadRepository _specialityRead,
        ISpecialityWriteRepository specialityWrite,
        IMapper _mapper) : ISpecialityService
    {
        public Task<IResult> CreateAsync(CreateCafedraDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<GetCafedraDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<GetCafedraDTO>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> HardDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RestoreAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SoftDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(UpdateCafedraDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
