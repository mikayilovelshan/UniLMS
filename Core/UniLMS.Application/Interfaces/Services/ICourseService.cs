using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.DTOs.Courses;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ICourseService
    {
        Task<IDataResult<List<GetCourseDTO>>> GetAllAsync();

        Task<IDataResult<GetCourseDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateCourseDTO model);

        Task<IResult> UpdateAsync(UpdateCourseDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
