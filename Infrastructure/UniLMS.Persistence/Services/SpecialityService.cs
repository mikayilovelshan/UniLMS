using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.DTOs.Specialities;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Application.Repositories.SpecialityRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Services
{
    public class SpecialityService(ISpecialityReadRepository _specialityRead,
        ISpecialityWriteRepository _specialityWrite,
        ICourseReadRepository _courseRead,
        IMapper _mapper,
        AppDbContext _context) : ISpecialityService
    {
        public async Task<IResult> CreateAsync(CreateSpecialityDTO model)
        {
            bool codeExists = await _specialityRead.GetWhere(s => s.Code == model.Code, tracking: false)
                .AnyAsync();
            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu ixtisas mövcuddur");

            var speciality = _mapper.Map<Speciality>(model);

            if(model.CourseIds != null && model.CourseIds.Any())
            {
                var courses = await _courseRead.GetWhere(x => model.CourseIds.Contains(x.Id), tracking: true)
                    .ToListAsync();

                foreach(var course in courses)
                {
                    _context.Entry(course).State = EntityState.Unchanged;
                }
                speciality.Courses = courses;
            }

            await _specialityWrite.AddAsync(speciality);
            await _specialityWrite.SaveAsync();

            return new SuccessResult("İxtisas əlavə edildi");
        }

        public async Task<IDataResult<List<GetSpecialityDTO>>> GetAllAsync()
        {
            var specialites = await _specialityRead
                .GetAll(tracking: false)
                .Include(s => s.Faculty)
                .Include(s => s.Cafedra)
                .Include(s => s.Courses)
                    .ThenInclude(s => s.Cafedra)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetSpecialityDTO>>(specialites);

            return new SuccessDataResult<List<GetSpecialityDTO>>(dtos);
        }

        public async Task<IDataResult<GetSpecialityDTO>> GetByIdAsync(Guid id)
        {
            var speciality = await _specialityRead
                .GetWhere(s => s.Id == id, tracking: false)
                .Include(s => s.Faculty)
                .Include(s => s.Cafedra)
                .Include(s => s.Courses) 
                .FirstOrDefaultAsync();

            if(speciality == null)
                return new ErrorDataResult<GetSpecialityDTO>("İxtisas tapılmadı.");

            var dto = _mapper.Map<GetSpecialityDTO>(speciality);

            return new SuccessDataResult<GetSpecialityDTO>(dto);

        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _specialityWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun ixtisas mövcud deyil");

            await _specialityWrite.SaveAsync();

            return new SuccessResult("İxtisas bazadan tamamilə silindi.");

        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var specialities = await _specialityRead
                .GetWhere(s => ids.Contains(s.Id),tracking: true)
                .ToListAsync();

            if(specialities == null)
                return new ErrorResult("Silmək üçün heç bir ixtisas tapılmadı");

            _specialityWrite.HardDeleteRange(specialities);

            await _specialityWrite.SaveAsync();

            return new SuccessResult("Seçilmiş ixtisaslar bazadan tamamilə silindi.");
        }

    

        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _specialityWrite.SoftDeleteAsync(id);
            if (!isRemoved)
                return new ErrorResult("Uyğun kafedra tapılmadı");

            await _specialityWrite.SaveAsync();
            return new SuccessResult("Kafedra müvəqqəti silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var cafedras = await _specialityRead.GetWhere(d => ids.Contains(d.Id), tracking: true).ToListAsync();
            if (cafedras == null)
                return new ErrorResult("Silmək üçün heç bir kafedra tapılmadı");
            _specialityWrite.SoftDeleteRange(cafedras);
            await _specialityWrite.SaveAsync();
            return new SuccessResult("Kafedralar tamamilə silindi");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _specialityWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _specialityWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateAsync(UpdateSpecialityDTO model)
        {
            var speciality = await _specialityRead.GetWhere(s => s.Id == model.Id, tracking: true)
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(); 

            if (speciality == null)
                return new ErrorResult("Uyğun ixtisas mövcud deyil");

            bool codeExists = await _specialityRead
                .GetWhere(s => s.Code == model.Code && s.Id != model.Id,tracking: false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu ixtisas artıq mövcuddur");

            _mapper.Map(model, speciality);

            if(model.CourseIds != null)
            {
                var updatedCourses = await _courseRead
                    .GetWhere(x => model.CourseIds.Contains(x.Id), tracking: true)
                    .ToListAsync();

                speciality.Courses.Clear();
            }

            await _specialityWrite.SaveAsync();

            return new SuccessResult("Ixtisas uğurla yeniləndi");
        }
    }
}
