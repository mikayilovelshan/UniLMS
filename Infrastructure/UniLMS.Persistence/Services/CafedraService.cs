using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CafedraRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class CafedraService(ICafedraReadRepository _cafedraRead,
        ICafedraWriteRepository _cafedraWrite,
        IMapper _mapper) : ICafedraService
    {
        public async Task<IResult> CreateAsync(CreateCafedraDTO model)
        {
            bool codeExists = await _cafedraRead
                .GetWhere(d => d.Code == model.Code, tracking : false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu kafedra mövcuddur");
            var cafedra = _mapper.Map<Cafedra>(model);
            await _cafedraWrite.AddAsync(cafedra);
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kafedra uğurla əlavə edildi");
        }

        public async Task<IDataResult<List<GetCafedraDTO>>> GetAllAsync()
        {
            var cafedras = await _cafedraRead.GetAll(tracking: false)
                .Include(f => f.Faculty)
                .ToListAsync();
            var dtos = _mapper.Map < List < GetCafedraDTO >> (cafedras);

            return new SuccessDataResult<List<GetCafedraDTO>>(dtos);
        }

        public async Task<IDataResult<GetCafedraDTO>> GetByIdAsync(Guid id)
        {
            var cafedra = await _cafedraRead.GetWhere(d => d.Id == id)
                 .Include(d => d.Faculty)
                 .FirstOrDefaultAsync();

            if (cafedra == null)
                return new ErrorDataResult<GetCafedraDTO>("Kafedra tapıla bilmədi");
            var mapped = _mapper.Map<GetCafedraDTO>(cafedra);
            return new SuccessDataResult<GetCafedraDTO>(mapped);

        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _cafedraWrite.HardDeleteAsync(id);
            if (!isRemoved)
                return new ErrorResult("Uyğun fakültə mövcud deyil");
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kaferda uğurla silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var cafedras = await _cafedraRead.GetWhere(d => ids.Contains(d.Id), tracking: true).ToListAsync();
            if (cafedras == null)
                return new ErrorResult("Silmək üçün heç bir kafedra tapılmadı");
            _cafedraWrite.HardDeleteRange(cafedras);
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kafedralar tamamilə silindi");
        }

 

        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _cafedraWrite.SoftDeleteAsync(id);
            if (!isRemoved)
                return new ErrorResult("Uyğun kafedra tapılmadı");

            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kafedra müvəqqəti silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var cafedras = await _cafedraRead.GetWhere(d => ids.Contains(d.Id), tracking: true).ToListAsync();
            if (cafedras == null)
                return new ErrorResult("Silmək üçün heç bir kafedra tapılmadı");
            _cafedraWrite.SoftDeleteRange(cafedras);
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kafedralar tamamilə silindi");
        }

        public async Task<IResult> UpdateAsync(UpdateCafedraDTO model)
        {
            var cafedra = await _cafedraRead.GetByIdAsync(model.Id, tracking: true);
            if (cafedra == null)
                return new ErrorResult("Uyğun kafedra mövcud deyil");

            bool codeExist = await _cafedraRead
                .GetWhere(d => d.Code == model.Code, tracking: false)
                .AnyAsync();
            if (codeExist)
                return new ErrorResult($"{model.Code} kodlu kafedra artıq mövcuddur");
            _mapper.Map(model, cafedra);
            _cafedraWrite.Update(cafedra);
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Kafedra yeniləndi");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _cafedraWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _cafedraWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }
    }
}
