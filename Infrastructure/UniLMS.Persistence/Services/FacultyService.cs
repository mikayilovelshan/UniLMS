using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Faculties;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.FacultyRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class FacultyService(IFacultyReadRepository _facultyRead,
        IFacultyWriteRepository _facultyWrite,
        IMapper _mapper) : IFacultyService
    {
        public async Task<IResult> CreateAsync(CreateFacultyDTO model)
        {
            bool codeExists = await _facultyRead
                .GetWhere(f => f.Code == model.Code, tracking: false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu fakültə artıq mövcuddur.");
            var faculty = _mapper.Map<Faculty>(model);
            await _facultyWrite.AddAsync(faculty);
            await _facultyWrite.SaveAsync();

            return new SuccessResult("Fakültə uğurla əlavə edildi");
        }


        public async Task<IDataResult<List<GetFacultyDTO>>> GetAllAsync()
        {
            var faculties = await _facultyRead.GetAll(tracking: false).ToListAsync();
            var dtos = _mapper.Map<List<GetFacultyDTO>>(faculties);

            return new SuccessDataResult<List<GetFacultyDTO>>(dtos);
        }

   

        public async Task<IDataResult<GetFacultyDTO>> GetByIdAsync(Guid id)
        {
            var faculty = await _facultyRead.GetByIdAsync(id, tracking : false);

            if (faculty == null)
                return new ErrorDataResult<GetFacultyDTO>("Fakültə tapıla bilmədi");

            var dto = _mapper.Map<GetFacultyDTO>(faculty);

            return new SuccessDataResult<GetFacultyDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _facultyWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun fakültə mövcud deyil");

            await _facultyWrite.SaveAsync();

            return new SuccessResult("Fakültə uğurla bazadan tamamilə silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var faculties = await _facultyRead.GetWhere(f => ids.Contains(f.Id), tracking: true).ToListAsync();

            if (faculties == null)
                return new ErrorResult("Silinmək üçün heç bir fakültə tapılmadı.");

            _facultyWrite.HardDeleteRange(faculties);
            await _facultyWrite.SaveAsync();
            return new SuccessResult("Fakültələrin hamısı tamamilə bazadan silindi");
        }



        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _facultyWrite.SoftDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun fakültə mövcud deyil");

            await _facultyWrite.SaveAsync();

            return new SuccessResult("Fakültə uğurla silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var faculties = await _facultyRead.GetWhere(f => ids.Contains(f.Id), tracking: true).ToListAsync();
            if (faculties == null)
                return new ErrorResult("Silinmək üçün heç bir fakültə tapılmadı.");

            _facultyWrite.SoftDeleteRange(faculties);
           await  _facultyWrite.SaveAsync();

            return new SuccessResult("Fakültələrin hamısı müvəqqəti silindi");

        }

        public async Task<IResult> UpdateAsync(UpdateFacultyDTO model)
        {
            var faculty = await _facultyRead.GetByIdAsync(model.Id, tracking: true);

            if (faculty == null)
                return new ErrorResult("Uyğun fakültə mövcud deyil");

            bool codeExists = await _facultyRead
                .GetWhere(f => f.Code == model.Code, tracking: false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu fakültə artıq mövcuddur");

            _mapper.Map(model, faculty);
            _facultyWrite.Update(faculty);

            await _facultyWrite.SaveAsync();

            return new SuccessResult("Fakültə məlumatları yeniləndi");
                
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _facultyWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadi və ya aktivdir");
            await _facultyWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }
    }
}
