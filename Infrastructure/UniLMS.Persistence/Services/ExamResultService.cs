using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Application.Repositories.ExamResultRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class ExamResultService(IExamResultReadRepository _readExamResult,
        IExamResultWriteRepository _writeExamResult,
        ICourseOfferingReadRepository _courseOfferingRead,
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

            return new SuccessResult("İmtahan qiynmətləri uğurla əalvə edildi");
        }
        
        public async Task<IDataResult<List<GetExamResultDTO>>> GetAllAsync()
        {
            var examResults = await _readExamResult.GetAll(tracking: false).ToListAsync();

            if (examResults == null)
                return new ErrorDataResult<List<GetExamResultDTO>>("Məlumat tapıla bilmədi");

            var dtos = _mapper.Map<List<GetExamResultDTO>>(examResults);

            return new SuccessDataResult<List<GetExamResultDTO>>(dtos);
        }

        public async Task<IDataResult<GetExamResultDTO>> GetByIdAsync(Guid id)
        {
            var examResult = await _readExamResult.GetByIdAsync(id);

            if(examResult == null)
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
    }
}
