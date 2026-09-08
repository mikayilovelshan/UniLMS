using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Groups;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface IGroupService
    {
        Task<IDataResult<List<GetGroupDTO>>> GetAllAsync();

        Task<IDataResult<GetGroupDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateGroupDTO model);

        Task<IResult> UpdateAsync(UpdateGroupDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
