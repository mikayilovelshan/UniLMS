using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities;
using UniLMS.Application.DTOs.CourseOfferings;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Application.Repositories.GroupRepository;
using UniLMS.Application.Repositories.SemesterRepository;
using UniLMS.Application.Repositories.TeacherRepository;
using UniLMS.Application.Utilities.Results;



namespace UniLMS.Persistence.Services
{
    public class CourseOfferingService(ICourseOfferingReadRepository _courseOfferingRead,
        ICourseOfferingWriteRepository _courseOfferingWrite,
        ISemesterReadRepository _semesterRead,
        IGroupReadRepository _groupRead,
        ICourseReadRepository _courseRead,
        ITeacherReadRepository _teacherRead,
        IMapper _mapper) : ICourseOfferingService
    {
        public async Task<IResult> CreateAsync(CreateCourseOfferingDTO model)                   
        {
            var semester = await _semesterRead.GetByIdAsync(model.SemesterId);

            if (semester == null)
                return new ErrorResult("Daxil edilən semestr mövcud deyil.");

            if (!semester.IsActive)
                return new ErrorResult($"Seçilmiş semestr ('{semester.Name}') aktiv deyil.");

            var group = _groupRead.GetWhere(d => d.Id == model.GroupId).AnyAsync();
            var course = _courseRead.GetWhere(c => c.Id == model.CourseId).AnyAsync();
            var teacher = _teacherRead.GetWhere(t => t.Id == model.TeacherId).AnyAsync();
            var isDuplicate = _courseOfferingRead.GetWhere(co => co.SemesterId == model.SemesterId &&
                                                           co.GroupId == model.GroupId && 
                                                           co.CourseId == model.CourseId &&
                                                           co.TeacherId == model.TeacherId).AnyAsync();

            await Task.WhenAll(group, course, teacher, isDuplicate);

            if (!await group) return new ErrorResult("Daxil edilən qrup mövcud deyil.");
            if (!await course) return new ErrorResult("Daxil edilən fənn mövcud deyil.");
            if (!await teacher) return new ErrorResult("Daxil edilən müəllim mövcud deyil.");
            if (await isDuplicate) return new ErrorResult("Bu semestrdə həmin qrup üçün göstərilən fənn artıq təyin edilib.");

            var courseOffering = _mapper.Map<CourseOffering>(model);
            await _courseOfferingWrite.AddAsync(courseOffering);
            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Dərs təyinatı uğurla yaradıldı.");
        }

        public async Task<IDataResult<List<GetCourseOfferingDTO>>> GetAllAsync()
        {
            var courseOfferings = await _courseOfferingRead.GetAll(tracking : false)
                .Include(s => s.Semester)
                .Include(c => c.Course)
                .Include(t => t.Teacher)
                .Include(g => g.Group)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetCourseOfferingDTO>>(courseOfferings);

            return new SuccessDataResult<List<GetCourseOfferingDTO>>(dtos);


        }

        public async Task<IDataResult<GetCourseOfferingDTO>> GetByIdAsync(Guid id)
        {
             var courseOffering = await _courseOfferingRead.GetWhere(s => s.Id == id, tracking: false)
                   .Include(s => s.Semester)
                    .Include(c => c.Course)
                    .Include(t => t.Teacher)
                    .Include(g => g.Group)
                    .FirstOrDefaultAsync();

            if (courseOffering == null)
                return new ErrorDataResult<GetCourseOfferingDTO>("Məlumat tapılmadı");

            var dto = _mapper.Map<GetCourseOfferingDTO>(courseOffering);

            return new SuccessDataResult<GetCourseOfferingDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            var isRemoved = await _courseOfferingWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Məlumat tapılmadı");

            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Uğurla silindi");

        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("İD siyahısı boş ola bilməz");

            var courseOfferings = await _courseOfferingRead.GetWhere(x => ids.Contains(x.Id), tracking: true).ToListAsync();

            if (!courseOfferings.Any())
                return new ErrorResult("Məlumat tapılmadı");

            _courseOfferingWrite.HardDeleteRange(courseOfferings);

            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Seçilmiş məlumatlar tamamilə silindi");


        }

        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            var courseoffering = await _courseOfferingRead.GetByIdAsync(id, tracking : true);

            if (courseoffering == null)
                return new ErrorResult("Məlumat tapılmadı");

            _courseOfferingWrite.SoftDelete(courseoffering);

            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Məlumat uğurla arxivləndi");

            
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Silinəcək İD siyahısı boşdur");

            var courseOfferings = await _courseOfferingRead.GetWhere(x => ids.Contains(x.Id), tracking : true).ToListAsync();

            if (!courseOfferings.Any())
                return new ErrorResult("Məlumat tapılmadı");

            _courseOfferingWrite.SoftDeleteRange(courseOfferings);

            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Seçilmiş məlumatlar arxivləndi");

        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _courseOfferingWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _courseOfferingWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateAsync(UpdateCourseOfferingDTO model)
        {
            var courseOffering = await _courseOfferingRead.GetWhere(co => co.Id == model.Id, tracking : true).FirstOrDefaultAsync();

            if (courseOffering == null)
                return new ErrorResult("Məlumat tapılmadı");

            var semester = await _semesterRead.GetByIdAsync(model.SemesterId);

            if (semester == null)
                return new ErrorResult("Daxil edilən semestr mövcud deyil.");

            if (!semester.IsActive)
                return new ErrorResult($"Seçilmiş semestr ('{semester.Name}') aktiv deyil.");

            var group = _groupRead.GetWhere(d => d.Id == model.GroupId).AnyAsync();
            var course = _courseRead.GetWhere(c => c.Id == model.CourseId).AnyAsync();
            var teacher = _teacherRead.GetWhere(t => t.Id == model.TeacherId).AnyAsync();
            var isDuplicate = _courseOfferingRead.GetWhere(co => co.Id != model.Id && co.SemesterId == model.SemesterId &&
                                                           co.GroupId == model.GroupId && co.CourseId == model.CourseId &&
                                                           co.TeacherId == model.TeacherId).AnyAsync();

            await Task.WhenAll(group, course, teacher, isDuplicate);

            if (!await group) return new ErrorResult("Daxil edilən qrup mövcud deyil.");
            if (!await course) return new ErrorResult("Daxil edilən fənn mövcud deyil.");
            if (!await teacher) return new ErrorResult("Daxil edilən müəllim mövcud deyil.");
            if (await isDuplicate) return new ErrorResult("Bu semestrdə həmin qrup üçün göstərilən fənn artıq təyin edilib.");

            _mapper.Map(model, courseOffering);

            await _courseOfferingWrite.SaveAsync();

            return new SuccessResult("Uğurla yeniləndi");
        }
    }
}
