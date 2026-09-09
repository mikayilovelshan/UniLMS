using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Semesters;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.SemesterRepository;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Persistence.Services
{
    public class SemesterService(ISemesterReadRepository _semesterRead,
        ISemesterWriteRepository _semesterWrite,
        IMapper _mapper) : ISemesterService
    {
        public Task<IResult> CreateAsync(CreateSemesterDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<GetSemesterDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<GetSemesterDTO>> GetByIdAsync(Guid id)
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

        public Task<IResult> UpdateAsync(UpdateSemesterDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
