using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Semesters;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.SemesterRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class SemesterService(ISemesterReadRepository _semesterRead,
        ISemesterWriteRepository _semesterWrite,
        IMapper _mapper) : ISemesterService
    {
        public async Task<IResult> CreateAsync(CreateSemesterDTO model)
        {
            if (model == null)
                return new ErrorResult("Lazım olan məlumatları daxil edin");

            if (model.StartDate >= model.EndDate)
                return new ErrorResult("Semestrin başlanğıc tarixi bitiş tarixindən əvvəl olmalıdır");

            if(model.IsActive == true)
            {
                var activeSemester = await _semesterRead.GetWhere(s => s.IsActive == true, tracking: true).FirstOrDefaultAsync();
                
                if(activeSemester != null)
                    activeSemester.IsActive = false;
            }

            var semester = _mapper.Map<Semester>(model);
            await _semesterWrite.AddAsync(semester);
            await _semesterWrite.SaveAsync();

            return new SuccessResult("Semestr uğurla əlavə olundu");
                
        }

        public async Task<IDataResult<List<GetSemesterDTO>>> GetAllAsync()
        {
            var semesters = await _semesterRead.GetAll(tracking: false)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetSemesterDTO>>(semesters);

            return new SuccessDataResult<List<GetSemesterDTO>>(dtos);
        }

        public async Task<IDataResult<GetSemesterDTO>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new ErrorDataResult<GetSemesterDTO>("Keçərli bir ID daxil edin.");

            var semester = await _semesterRead.GetByIdAsync(id, tracking: false);

            if (semester == null)
                return new ErrorDataResult<GetSemesterDTO>("Semestr tapılmadı.");

            var dto = _mapper.Map<GetSemesterDTO>(semester);

            return new SuccessDataResult<GetSemesterDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new ErrorResult("Keçərli bir semestr ID-si daxil edin.");

            bool isRemoved = await _semesterWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun semester mövcud deyil");
            
            await _semesterWrite.SaveAsync();

            return new SuccessResult("Semester uğurla silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Silinməsi üçün ən azı bir semestr ID-si daxil edilməlidir.");

            var semesters = await _semesterRead.GetWhere(s => ids.Contains(s.Id), tracking: true).ToListAsync();

            if (semesters == null || !semesters.Any())
                return new ErrorResult("Silinməsi üçün semesterlər tapılmadı");

            if (semesters.Count != ids.Distinct().Count())
                return new ErrorResult("Göndərilən semestrlərdən bəziləri bazada tapılmadı.");

            _semesterWrite.HardDeleteRange(semesters);

            await _semesterWrite.SaveAsync();

            return new SuccessResult("Seçilən semestrlər uğurla silindi.");
        }


        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new ErrorResult("Keçərli bir semestr ID-si daxil edin.");

            var semester = await _semesterRead.GetByIdAsync(id, tracking: true);

            if (semester == null)
                return new ErrorResult("Uyğun semestr tapılmadı.");

            if (semester.IsActive)
                semester.IsActive = false;

            bool isRemoved = await _semesterWrite.SoftDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun semester mövcud deyil");

            await _semesterWrite.SaveAsync();

            return new SuccessResult("Semester uğurla silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Silinməsi üçün ən azı bir semestr ID-si daxil edilməlidir.");

            var semesters = await _semesterRead.GetWhere(s => ids.Contains(s.Id), tracking: true).ToListAsync();

            if(semesters == null || !semesters.Any())
                return new ErrorResult("Silinməsi üçün semesterlər tapılmadı");

            if (semesters.Count != ids.Distinct().Count())
                return new ErrorResult("Göndərilən semestrlərdən bəziləri bazada tapılmadı.");

            foreach (var semester in semesters)
                semester.IsActive = false;

            _semesterWrite.SoftDeleteRange(semesters);
            await _semesterWrite.SaveAsync();

            return new SuccessResult("Seçilən semestrlər müvəqqəti silindi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _semesterWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadi və ya aktivdir");
            await _semesterWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateAsync(UpdateSemesterDTO model)
        {
            if (model == null || model.Id == Guid.Empty) 
            {
                return new ErrorResult("Keçərli semestr məlumatı daxil edin.");
            }

            if (model.StartDate >= model.EndDate)
                return new ErrorResult("Semestrin başlanğıc tarixi bitiş tarixindən əvvəl olmalıdır.");

            var semester = await _semesterRead.GetByIdAsync(model.Id, tracking : true);

            if (semester == null)
                return new ErrorResult("Semestr tapılmadı.");

            if (model.IsActive)
            {
                var activeSemester = await _semesterRead.GetWhere(s => s.IsActive == true && s.Id != model.Id, tracking: true).FirstOrDefaultAsync();

                if (activeSemester != null)
                    activeSemester.IsActive = false;
            }
            _mapper.Map(model, semester);
            await _semesterWrite.SaveAsync();

            return new SuccessResult("Semestr məlumatları uğurla yeniləndi.");

        }
    }
}
