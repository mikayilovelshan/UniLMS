using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Application.Interfaces.Services
{
    public interface IExamResultService
    {
        Task<IDataResult<List<GetExamResultDTO>>> GetAllAsync();

        Task<IDataResult<GetExamResultDTO>> GetByIdAsync(Guid id);

        Task<IDataResult<StudentExamResultSummaryDTO>> GetStudentExamResultSummary(Guid studentId, Guid courseOfferingId);

        Task<IDataResult<StudentCourseDetailedReportDTO>> GetStudentCourseDetailedReport(Guid studentId, Guid courseOfferingId);

        Task<IResult> CreateRangeAsync(CreateExamResultRangeDTO model);

        Task<IResult> UpdateAsync(UpdateExamResultDTO model);

        Task<IResult> SoftDeleteAsync(Guid id);

        Task<IResult> HardDeleteAsync(Guid id);

        Task<IResult> SoftDeleteRangeAsync(List<Guid> ids);

        Task<IResult> HardDeleteRangeAsync(List<Guid> ids);

        Task<IResult> RestoreAsync(Guid id);
    }
}
