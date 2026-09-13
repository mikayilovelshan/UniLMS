using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseOfferings;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ICourseOfferingService
    {
        Task<IDataResult<List<GetCourseOfferingDTO>>> GetAllAsync();

        Task<IDataResult<GetCourseOfferingDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateCourseOfferingDTO model);

        Task<IResult> UpdateAsync(UpdateCourseOfferingDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
