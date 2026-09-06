using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.DTOs.Courses;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Repositories.CourseRepository;

namespace UniLMS.Persistence.Services
{
    public class CourseService(ICourseReadRepository _courseRead,
        ICourseWriteRepository _courseWrite,
        IMapper _mapper) : ICourseService
    {
        public async Task<IResult> CreateAsync(CreateCourseDTO model)
        {
            bool codeExists = await _courseRead
                .GetWhere(x => x.Code == model.Code, tracking : false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu kurs artıq mövcuddur");

            var course = _mapper.Map<Course>(model);
            await _courseWrite.AddAsync(course);
            await _courseWrite.SaveAsync();

            return new SuccessResult("Kurs uğurla əlavə edildi");
        }

        public async Task<IDataResult<List<GetCourseDTO>>> GetAllAsync()
        {
            var courses = await _courseRead.GetAll(tracking: false)
                .Include(x => x.Cafedra)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetCourseDTO>>(courses);

            return new SuccessDataResult<List<GetCourseDTO>>(dtos);
        }
        public async Task<IDataResult<GetCourseDTO>> GetByIdAsync(Guid id)
        {
            var course = await _courseRead.GetWhere(x => x.Id == id)
                .Include(x => x.Cafedra)
                .FirstOrDefaultAsync();

            if (course == null)
                return new ErrorDataResult<GetCourseDTO>("Kurs tapıla bilmədi");
            
            var dto = _mapper.Map<GetCourseDTO>(course);

            return new SuccessDataResult<GetCourseDTO>(dto); 
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _courseWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun kurs mövcud deyil");

            await _courseWrite.SaveAsync();
            return new SuccessResult("Kaferda uğurla silindi");

        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var courses = await _courseRead.GetWhere(x => ids.Contains(x.Id), tracking: true)
                .ToListAsync();

            if (courses == null)
                return new ErrorResult("Silmək üçün heç bir kurs tapılmadı");
            _courseWrite.HardDeleteRange(courses);
            await _courseWrite.SaveAsync();
            return new SuccessResult("Kurslar tamamilə silindi");
        }


        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _courseWrite.SoftDeleteAsync(id);
            if (!isRemoved)
                return new ErrorResult("Uyğun kurs tapılmadı");

            await _courseWrite.SaveAsync();
            return new SuccessResult("Kurs müvəqqəti silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var courses = await _courseRead.GetWhere(d => ids.Contains(d.Id), tracking: true).ToListAsync();
            if (courses == null)
                return new ErrorResult("Silmək üçün heç bir kurs tapılmadı");
            _courseWrite.SoftDeleteRange(courses);
            await _courseWrite.SaveAsync();
            return new SuccessResult("Kurslar müvəqqəti silindi");
        }


        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _courseWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _courseWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }



        public async Task<IResult> UpdateAsync(UpdateCourseDTO model)
        {
            var course = await _courseRead.GetByIdAsync(model.Id, tracking : true);

            if (course == null)
                return new ErrorResult("Uyğun kurs mövcud deyil");

            bool codeExists = await _courseRead
                .GetWhere(x => x.Code == model.Code, tracking: false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu kurs artıq mövcuddur");
            _mapper.Map(model, course);
            _courseWrite.Update(course);
            await _courseWrite.SaveAsync();
            return new SuccessResult("Kurs uğurla yeniləndi");
        }
    }
}
