using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.AttendanceRepository;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Application.Repositories.ExamResultRepository;
using UniLMS.Application.Repositories.StudentRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;
using UniLMS.Domain.Enums;

namespace UniLMS.Persistence.Services
{
    public class ExamResultService(IExamResultReadRepository _readExamResult,
        IExamResultWriteRepository _writeExamResult,
        ICourseOfferingReadRepository _courseOfferingRead,
        IStudentReadRepository _studentRead,
        IAttendanceReadRepository _attendanceRead,
        IMapper _mapper) : IExamResultService
    {
        public async Task<IResult> CreateRangeAsync(CreateExamResultRangeDTO model)
        {
            var courseOffering = await _courseOfferingRead.GetByIdAsync(model.CourseOfferingId, tracking: false);

            if (courseOffering == null)
                return new ErrorResult("Uyğun dərs qeydi tapılmadı");

            var examResult = await _readExamResult.GetWhere(x => x.CourseOfferingId == model.CourseOfferingId && 
                                                        x.ExamType == model.ExamType && 
                                                        x.EvaluationDate.Date == model.EvaluationDate.Date, tracking: false).AnyAsync();

            if (examResult)
                return new ErrorResult("Bu tarixdə artıq imtahan keçirilib və qiymətlər daxil edilib");

            var studentsId = model.StudentScores.Select(x => x.StudentId).ToList();

            var validStudents = await _courseOfferingRead.GetWhere(x => x.Id == model.CourseOfferingId, tracking : false)
                .SelectMany(x => x.Group.Students)
                .Where(x => !x.IsDeleted)
                .Select(x => x.Id)
                .ToListAsync();

            var invalidStudentsId = studentsId.Except(validStudents).ToList();

            if (invalidStudentsId.Any())
                return new ErrorResult("Göndərilən tələbələrdən bir və ya bir neçəsi bu qrupa aid deyil.");

            var examResults = model.StudentScores.Select(s => new ExamResult
            {
                Id = Guid.NewGuid(),
                CourseOfferingId = model.CourseOfferingId,
                ExamType = model.ExamType,
                EvaluationDate = model.EvaluationDate,
                StudentId = s.StudentId,
                Score = s.Score
            }).ToList();

            await _writeExamResult.AddRangeAsync(examResults);
            await _writeExamResult.SaveAsync();

            return new SuccessResult("İmtahan qiymətləri uğurla əalvə edildi");
        }
        
        public async Task<IDataResult<List<GetExamResultDTO>>> GetAllAsync()
        {
            var examResults = await _readExamResult.GetAll(tracking: false)
                .Include(x => x.Student)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Course)
                .ToListAsync();

            if (examResults == null)
                return new ErrorDataResult<List<GetExamResultDTO>>("Məlumat tapıla bilmədi");

            var dtos = _mapper.Map<List<GetExamResultDTO>>(examResults);

            return new SuccessDataResult<List<GetExamResultDTO>>(dtos);
        }

        public async Task<IDataResult<GetExamResultDTO>> GetByIdAsync(Guid id)
        {
            var examResult = await _readExamResult.GetWhere(x => x.Id == id, tracking : false)
                .Include(x => x.Student)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync();

            if (examResult == null)
                return new ErrorDataResult<GetExamResultDTO>("Məlumat tapıla bilmədi");

            var dto = _mapper.Map<GetExamResultDTO>(examResult);

            return new SuccessDataResult<GetExamResultDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _writeExamResult.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Silinmə əməliyyatı uğursuz oldu");

            await _writeExamResult.SaveAsync();

            return new SuccessResult("Məlumat tamamilə silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var examResults = await _readExamResult.GetWhere(x => ids.Contains(x.Id), tracking: true).ToListAsync();

            if (!examResults.Any())
                return new ErrorResult("Silinəcək məlumat tapılmadı");

            if (examResults.Count != ids.Count)
                return new ErrorResult("Göndərilən məlumatların bir və ya bir neçəsi sistemdə tapılmadı.");

            _writeExamResult.HardDeleteRange(examResults);

            await _writeExamResult.SaveAsync();

            return new SuccessResult("Məlumatlar bazadan birdəfəlik silindi.");
        }


        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _writeExamResult.SoftDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Silinmə əməliyyatı uğursuz oldu");

            await _writeExamResult.SaveAsync();

            return new SuccessResult("Məlumatlar bazadan müvəqqəti silindi.");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var examResults = await _readExamResult.GetWhere(x => ids.Contains(x.Id), tracking: true).ToListAsync();

            if (!examResults.Any())
                return new ErrorResult("Silinəcək məlumat tapılmadı");

            if (examResults.Count != ids.Count)
                return new ErrorResult("Göndərilən məlumatların bir və ya bir neçəsi sistemdə tapılmadı.");

            _writeExamResult.SoftDeleteRange(examResults);

            await _writeExamResult.SaveAsync();

            return new SuccessResult("Məlumatlar bazadan müvəqqəti silindi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _writeExamResult.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _writeExamResult.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateAsync(UpdateExamResultDTO model)
        {
            var examResult = await _readExamResult.GetByIdAsync(model.Id, tracking : true);

            if (examResult == null)
                return new ErrorResult("Uyğun məlumat mövcud deyil");

            _mapper.Map(model, examResult);
            await _writeExamResult.SaveAsync();

            return new SuccessResult("Dəyişiklik tətbiq edildi");

        }

        public async Task<IDataResult<StudentExamResultSummaryDTO>> GetStudentExamResultSummary(Guid studentId, Guid courseOfferingId)
        {
            var student = await _studentRead.GetByIdAsync(studentId, tracking: false);

            if (student == null)
                return new ErrorDataResult<StudentExamResultSummaryDTO>("Tələbə tapılmadı.");

            var courseOffering = await _courseOfferingRead.GetByIdAsync(courseOfferingId, tracking: false);

            if (courseOffering == null)
                return new ErrorDataResult<StudentExamResultSummaryDTO>("Tədris olunan dərs tapılmadı.");

            var examResults = await _readExamResult.GetWhere(x => x.StudentId == studentId && x.CourseOfferingId == courseOfferingId, tracking: false ).ToListAsync();

            var attendances = await _attendanceRead.GetWhere(x => x.StudentId == studentId && x.CourseSchedule.CourseOfferingId == courseOfferingId, tracking: false).ToListAsync();

            int totalClasses = attendances.Count();
            int attendandClass = attendances.Count(x => x.IsPresent);

            decimal attendanceScore = 0;

            if(totalClasses > 0)
                attendanceScore = ((decimal)attendandClass / totalClasses) * 10;

            decimal quizAndExamScore = examResults.Where(x => x.ExamType != ExamType.FinalExam).Sum(x => x.Score);

            decimal entryScore = quizAndExamScore + attendanceScore;

            decimal finalScore = examResults.Where(x => x.ExamType == ExamType.FinalExam).Select(x => x.Score).FirstOrDefault();

            decimal totalScore = entryScore + finalScore;

            bool isPassed = totalScore >= 51 && finalScore >= 17;

            string letterGrade = (isPassed, totalScore) switch
            {
                (false, _) => "F",
                (_, >= 91) => "A",
                (_, >= 81) => "B",
                (_, >= 71) => "C",
                (_, >= 61) => "D",
                (_, >= 51) => "E",

                _ => "F"
            };


            var summaryDto = _mapper.Map<StudentExamResultSummaryDTO>(student);
            _mapper.Map(courseOffering, summaryDto);

            summaryDto.EntryScore = entryScore;
            summaryDto.FinalScore = finalScore;
            summaryDto.LetterGrade = letterGrade;
            summaryDto.IsPassed = isPassed;

            return new SuccessDataResult<StudentExamResultSummaryDTO>(summaryDto);
        }

        public async Task<IDataResult<StudentCourseDetailedReportDTO>> GetStudentCourseDetailedReport(Guid studentId, Guid courseOfferingId)
        {
            var student = await _studentRead.GetByIdAsync(studentId, tracking: false);
            if (student == null)
                return new ErrorDataResult<StudentCourseDetailedReportDTO>("Tələbə tapılmadı.");

            var courseOffering = await _courseOfferingRead.GetByIdAsync(courseOfferingId, tracking: false);
            if (courseOffering == null)
                return new ErrorDataResult<StudentCourseDetailedReportDTO>("Tədris olunan dərs tapılmadı.");

            
            var attendances = await _attendanceRead
                .GetWhere(x => x.StudentId == studentId && x.CourseSchedule.CourseOfferingId == courseOfferingId, tracking: false)
                .ToListAsync();

            int totalClasses = attendances.Count;
            int attendandClass = attendances.Count(x => x.IsPresent);

            decimal attendanceScore = 0;
            if (totalClasses > 0)
                attendanceScore = ((decimal)attendandClass / totalClasses) * 10;

            
            var rawExamResults = await _readExamResult
                .GetWhere(x => x.StudentId == studentId && x.CourseOfferingId == courseOfferingId, tracking: false)
                .Include(x => x.Student)
                    .ThenInclude(s => s.AppUser)
                .Include(x => x.CourseOffering)
                    .ThenInclude(co => co.Course)
                .OrderBy(x => x.EvaluationDate)
                .ToListAsync();

            
            decimal quizAndExamScore = rawExamResults
                .Where(x => x.ExamType != ExamType.FinalExam)
                .Sum(x => x.Score);

            decimal entryScore = quizAndExamScore + attendanceScore;

            decimal finalScore = rawExamResults
                .Where(x => x.ExamType == ExamType.FinalExam)
                .Select(x => x.Score)
                .FirstOrDefault();

            decimal totalScore = entryScore + finalScore;

            bool isPassed = totalScore >= 51 && finalScore >= 17;

            string letterGrade = (isPassed, totalScore) switch
            {
                (false, _) => "F",
                (_, >= 91) => "A",
                (_, >= 81) => "B",
                (_, >= 71) => "C",
                (_, >= 61) => "D",
                (_, >= 51) => "E",
                _ => "F"
            };

            
            var detailedScores = rawExamResults.Select(x => new GetExamResultDTO
            {
                Id = x.Id,
                ExamTypeName = x.ExamType.ToString(),
                Score = x.Score,
                EvaluationDate = x.EvaluationDate
            }).ToList();

            
            var courseResultReportDto = _mapper.Map<StudentCourseDetailedReportDTO>(student);
            _mapper.Map(courseOffering, courseResultReportDto);

            courseResultReportDto.EntryScore = entryScore;
            courseResultReportDto.FinalScore = finalScore;
            courseResultReportDto.LetterGrade = letterGrade;
            courseResultReportDto.IsPassed = isPassed;
            courseResultReportDto.DetailedScores = detailedScores;

            return new SuccessDataResult<StudentCourseDetailedReportDTO>(courseResultReportDto);
        }

    }
}
