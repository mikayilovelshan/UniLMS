using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Faculties;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services    
{
    public interface IFacultyService
    {
        Task<IDataResult<List<GetFacultyDTO>>> GetAllAsync();

        Task<IDataResult<GetFacultyDTO>> GetByIdAsync(Guid id);

        Task<IResult> CreateAsync(CreateFacultyDTO model);

        Task<IResult> UpdateAsync(UpdateFacultyDTO model);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);
        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);


    }
}
