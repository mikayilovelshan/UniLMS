using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseSchedules;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ICourseScheduleService
    {
        Task<IDataResult<List<GetCourseScheduleDTO>>> GetAllAsync();

        Task<IDataResult<GetCourseScheduleDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateCourseScheduleDTO model);

        Task<IResult> UpdateAsync(UpdateCourseScheduleDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
