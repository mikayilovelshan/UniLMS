using System;
using System.Collections.Generic;
using System.Text;

using UniLMS.Application.DTOs.Teachers;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ITeacherService 
    {
        Task<IDataResult<List<GetTeacherDTO>>> GetAllAsync();

        Task<IDataResult<GetTeacherDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateTeacherDTO model);

        Task<IResult> UpdateAsync(UpdateTeacherDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
