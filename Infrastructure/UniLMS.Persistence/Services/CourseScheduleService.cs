using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseSchedules;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Application.Repositories.CourseScheduleRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class CourseScheduleService(ICourseScheduleReadRepository _courseScheduleRead,
        ICourseScheduleWriteRepository _courseScheduleWrite,
        ICourseOfferingReadRepository _courseOfferingRead,
        IMapper _mapper) : ICourseScheduleService
    {
        public async Task<IResult> CreateAsync(CreateCourseScheduleDTO model)
        {
            if (model.StartTime >= model.EndTime)
                return new ErrorResult("Dərsin bitmə saatı başlanğıc saatından sonra olmalıdır.");

            var courseOffering = await _courseOfferingRead.GetWhere(co => co.Id == model.CourseOfferingId, tracking: false)
                .FirstOrDefaultAsync();

            if (courseOffering == null)
                return new ErrorResult("Seçilmiş dərs təyinatı tapılmadı.");

           var conflictSchedules = await _courseScheduleRead.GetWhere(s =>
                s.DayOfWeek == model.DayOfWeek &&
                s.StartTime < model.StartTime &&
                s.EndTime > model.EndTime &&
                (
                    s.CourseOffering.GroupId == courseOffering.GroupId ||
                    s.CourseOffering.TeacherId == courseOffering.TeacherId ||
                    s.CourseOffering.RoomCode == courseOffering.RoomCode), tracking: false)
                .Select(s => new
                {
                    s.CourseOffering.GroupId,
                    s.CourseOffering.TeacherId,
                    s.CourseOffering.RoomCode
                }).ToListAsync();

            if (conflictSchedules.Any())
            {
                if (conflictSchedules.Any(s => s.GroupId == courseOffering.GroupId))
                    return new ErrorResult("Həmin qrupun seçilmiş gün və saat aralığında artıq başqa dərsi var.");

                if (conflictSchedules.Any(s => s.TeacherId == courseOffering.TeacherId))
                    return new ErrorResult("Müəllimin seçilmiş gün və saat aralığında başqa qrupda dərsi var.");

                if (conflictSchedules.Any(s => s.RoomCode == courseOffering.RoomCode))
                    return new ErrorResult("Seçilmiş otaq belirtilən gün və saat aralığında boş deyil.");
            }
                

            var courseSchedule = _mapper.Map<CourseSchedule>(model);
            await _courseScheduleWrite.AddAsync(courseSchedule);
            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Dərs cədvəli uğurla əlavə edildi.");
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
