using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Groups;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.GroupRepository;
using UniLMS.Application.Repositories.StudentRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Services
{
    public class GroupService(IGroupReadRepository _groupRead,
        IGroupWriteRepository _groupWrite,
        IStudentReadRepository _studentRead,
        IStudentWriteRepository _studentWrite,
        IMapper _mapper) : IGroupService
    {
        public async Task<IResult> CreateAsync(CreateGroupDTO model)
        {
            bool codeExists = await _groupRead.GetWhere(x => x.Code == model.Code, tracking : false).AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu qrup mövcuddur");
            var group = new Group
            {
                Code = model.Code,
                SpecialityId = model.SpecialityId
            };

            await _groupWrite.AddAsync(group);
            await _groupWrite.SaveAsync();

               if(model.StudentIds != null && model.StudentIds.Any())
            {
                var students = await _studentRead.GetWhere(s => model.StudentIds.Contains(s.Id), tracking: true).ToListAsync();
                foreach (var student in students)
                {
                    student.GroupId = group.Id;
                    _studentWrite.Update(student);
                }
            }

            await _groupWrite.SaveAsync();
            return new SuccessResult("Qrup uğurla əlavə edildi.");
        }

        public async Task<IDataResult<List<GetGroupDTO>>> GetAllAsync()
        {
            var specialites = await _groupRead
                .GetAll(tracking: false)
                .Include(s => s.Speciality)
                .Include(s => s.Students)
                    .ThenInclude(s => s.AppUser)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetGroupDTO>>(specialites);

            return new SuccessDataResult<List<GetGroupDTO>>(dtos);
        }

        public async Task<IDataResult<GetGroupDTO>> GetByIdAsync(Guid id)
        {
            var group = await _groupRead
                .GetAll(tracking: false)
                .Include(s => s.Speciality)
                .Include(s => s.Students)
                    .ThenInclude(s => s.AppUser)
                .ToListAsync();

            if (group == null)
                return new ErrorDataResult<GetGroupDTO>("Qrup tapılmadı.");

            var dto = _mapper.Map<GetGroupDTO>(group);

            return new SuccessDataResult<GetGroupDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _groupWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Uyğun ixtisas mövcud deyil");

            await _groupWrite.SaveAsync();

            return new SuccessResult("İxtisas bazadan tamamilə silindi.");

        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var specialities = await _groupRead
                .GetWhere(s => ids.Contains(s.Id), tracking: true)
                .ToListAsync();

            if (specialities == null)
                return new ErrorResult("Silmək üçün heç bir qrup tapılmadı");

            _groupWrite.HardDeleteRange(specialities);

            await _groupWrite.SaveAsync();

            return new SuccessResult("Seçilmiş qruplar bazadan tamamilə silindi.");
        }



        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            bool isRemoved = await _groupWrite.SoftDeleteAsync(id);
            if (!isRemoved)
                return new ErrorResult("Uyğun qrup tapılmadı");

            await _groupWrite.SaveAsync();
            return new SuccessResult("Qrup müvəqqəti silindi");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var cafedras = await _groupRead.GetWhere(d => ids.Contains(d.Id), tracking: true).ToListAsync();
            if (cafedras == null)
                return new ErrorResult("Silmək üçün heç bir qrup tapılmadı");
            _groupWrite.SoftDeleteRange(cafedras);
            await _groupWrite.SaveAsync();
            return new SuccessResult("Qruplar passivləşdirildi");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _groupWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _groupWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }
        public async Task<IResult> UpdateAsync(UpdateGroupDTO model)
        {
            var group = await _groupRead.GetWhere(x => x.Id == model.Id, tracking: true)
                .Include(x => x.Speciality)
                .Include(x => x.Students)
                    .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync();

            if (group == null)
                return new ErrorResult("Uyğun qrup mövcud deyil");

            bool codeExists = await _groupRead
                .GetWhere(s => s.Code == model.Code && s.Id != model.Id, tracking: false)
                .AnyAsync();

            if (codeExists)
                return new ErrorResult($"{model.Code} kodlu qrup artıq mövcuddur");

            if(model.StudentIds != null)
            {
                var updatedStudents = await _studentRead.GetWhere(x => model.StudentIds.Contains(x.Id), tracking: true).ToListAsync();

                group.Students.Clear();

                foreach (var student in updatedStudents)
                    group.Students.Add(student);
            }

            await _groupWrite.SaveAsync();
            return new SuccessResult("Qrup uğurla yeniləndi");
        }
    }
}
