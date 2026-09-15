using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseSchedules;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Persistence.Services
{
    public class CourseScheduleService : ICourseScheduleService
    {
        public Task<IResult> CreateAsync(CreateCourseScheduleDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<GetCourseScheduleDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<GetCourseScheduleDTO>> GetByIdAsync(Guid id)
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

        public Task<IResult> UpdateAsync(UpdateCourseScheduleDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
