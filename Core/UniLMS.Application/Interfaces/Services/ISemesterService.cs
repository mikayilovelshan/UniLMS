using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Semesters;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ISemesterService
    {
        Task<IDataResult<List<GetSemesterDTO>>> GetAllAsync();

        Task<IDataResult<GetSemesterDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateSemesterDTO model);

        Task<IResult> UpdateAsync(UpdateSemesterDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
