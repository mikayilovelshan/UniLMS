using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<IDataResult<List<GetAttendanceDTO>>> GetAllAsync();

        Task<IDataResult<GetAttendanceDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateRangeAsync(CreateAttendanceRangeDTO model);

        Task<IResult> UpdateRangeAsync(UpdateAttendanceRangeDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
