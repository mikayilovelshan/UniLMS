using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Students;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<IDataResult<List<GetStudentDTO>>> GetAllAsync();

        Task<IDataResult<GetStudentDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateStudentDTO model);

        Task<IResult> UpdateAsync(UpdateStudentDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
