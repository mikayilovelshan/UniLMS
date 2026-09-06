using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Specialities;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface ISpecialityService
    {
        Task<IDataResult<List<GetSpecialityDTO>>> GetAllAsync();

        Task<IDataResult<GetSpecialityDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateSpecialityDTO model);

        Task<IResult> UpdateAsync(UpdateSpecialityDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
