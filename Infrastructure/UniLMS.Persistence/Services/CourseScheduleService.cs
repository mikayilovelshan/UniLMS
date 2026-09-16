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
                s.StartTime < model.EndTime &&
                s.EndTime > model.StartTime &&
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

        public async Task<IDataResult<List<GetCourseScheduleDTO>>> GetAllAsync()
        {
            var courseSchedule = await _courseScheduleRead.GetAll(tracking: false)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Group)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Course)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Teacher)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetCourseScheduleDTO>>(courseSchedule);

            return new SuccessDataResult<List<GetCourseScheduleDTO>>(dtos);
        }

        public async Task<IDataResult<GetCourseScheduleDTO>> GetByIdAsync(Guid id)
        {
            var courseSchedule = await _courseScheduleRead.GetWhere(x => x.Id == id, tracking : false)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Group)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Course)
                .Include(x => x.CourseOffering)
                    .ThenInclude(x => x.Teacher)
                .FirstOrDefaultAsync();

            if (courseSchedule == null)
                return new ErrorDataResult<GetCourseScheduleDTO>("Dərs cədvəli tapılmadı");

            var dto = _mapper.Map<GetCourseScheduleDTO>(courseSchedule);

            return new SuccessDataResult<GetCourseScheduleDTO>(dto);
                
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new ErrorResult("Keçərli bir dərs cədvəli ID-si daxil edin.");
            
            var isRemoved = await _courseScheduleWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun cədvəl mövcud deyil");
            
            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Uğurla silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Silinməsi üçün ən azı bir cədvəl ID-si daxil edilməlidir.");

            var distinctIds = ids.Distinct().ToList();

            var courseSchedules = await _courseScheduleRead
                .GetWhere(cs => distinctIds.Contains(cs.Id), tracking: true)
                .ToListAsync();

            if (courseSchedules == null || !courseSchedules.Any())
                return new ErrorResult("Göstərilən ID-lərə uyğun heç bir dərs cədvəli tapılmadı.");

            if (courseSchedules.Count != distinctIds.Count)
                return new ErrorResult("Göndərilən cədvəllərdən bəziləri bazada tapılmadı. Silinmə əməliyyatı ləğv edildi.");

            _courseScheduleWrite.HardDeleteRange(courseSchedules);
            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Seçilən dərs cədvəlləri bazadan uğurla silindi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _courseScheduleWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadi və ya aktivdir");
            await _courseScheduleWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new ErrorResult("Keçərli bir dərs cədvəli ID-si daxil edin.");

            var schedule = await _courseScheduleRead.GetByIdAsync(id.ToString(), tracking: true);

            if (schedule == null)
                return new ErrorResult("Uyğun dərs cədvəli tapılmadı.");

            bool isRemoved = await _courseScheduleWrite.SoftDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Dərs cədvəli arxivlənərkən xəta baş verdi.");

            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Dərs cədvəli uğurla arxivləndi.");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Arxivlənməsi üçün ən azı bir dərs cədvəli ID-si daxil edilməlidir.");

            var distinctIds = ids.Distinct().ToList();

            var schedules = await _courseScheduleRead
                .GetWhere(cs => distinctIds.Contains(cs.Id), tracking: true)
                .ToListAsync();

            if (schedules == null || !schedules.Any())
                return new ErrorResult("Göstərilən ID-lərə uyğun heç bir dərs cədvəli tapılmadı.");

            if (schedules.Count != distinctIds.Count)
                return new ErrorResult("Göndərilən dərs cədvəllərindən bəziləri bazada tapılmadı. Arxivləmə ləğv edildi.");

            _courseScheduleWrite.SoftDeleteRange(schedules);
            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Seçilən dərs cədvəlləri uğurla arxivləndi.");
        }

        public async Task<IResult> UpdateAsync(UpdateCourseScheduleDTO model)
        {
            if (model == null)
                return new ErrorResult("Məlumatları tam şəkildə daxil edin");

            if (model.StartTime >= model.EndTime)
                return new ErrorResult("Dərsin bitmə saatı başlanğıc saatından sonra olmalıdır.");

            var courseSchedule = await _courseScheduleRead.GetByIdAsync(model.Id, tracking : true);

            if (courseSchedule == null)
                return new ErrorResult("Dərs cədvəli tapılmadı");

            var courseOffering = await _courseOfferingRead.GetWhere(co => co.Id == model.CourseOfferingId, tracking: false).FirstOrDefaultAsync();

            if (courseOffering == null)
                return new ErrorResult("Hər hansı bir dərs təyinatı tapılmadı");

            var conflicts = await _courseScheduleRead.GetWhere(x => 
                x.Id != model.Id &&
                x.DayOfWeek == model.DayOfWeek &&
                x.StartTime < model.EndTime &&
                x.EndTime > model.StartTime &&
                (
                   x.CourseOffering.GroupId == courseOffering.GroupId ||
                   x.CourseOffering.TeacherId == courseOffering.TeacherId ||
                   x.CourseOffering.RoomCode == courseOffering.RoomCode), tracking: false)
                .Select(x => new
                {
                    x.CourseOffering.GroupId,
                    x.CourseOffering.TeacherId,
                    x.CourseOffering.RoomCode
                }).ToListAsync();

            if (conflicts.Any())
            {
                if (conflicts.Any(s => s.GroupId == courseOffering.GroupId))
                    return new ErrorResult("Həmin qrupun seçilmiş gün və saat aralığında artıq başqa dərsi var.");

                if (conflicts.Any(s => s.TeacherId == courseOffering.TeacherId))
                    return new ErrorResult("Müəllimin seçilmiş gün və saat aralığında başqa qrupda dərsi var.");

                if (conflicts.Any(s => s.RoomCode == courseOffering.RoomCode))
                    return new ErrorResult("Seçilmiş otaq belirtilən gün və saat aralığında boş deyil.");
            }
            
            _mapper.Map(model, courseSchedule);
            await _courseScheduleWrite.SaveAsync();

            return new SuccessResult("Dərs cədvəli uğurla yeniləndi.");
        }
    }
}
