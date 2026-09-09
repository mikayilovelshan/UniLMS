using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Teachers;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.TeacherRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class TeacherService(ITeacherReadRepository _teacherRead,
        ITeacherWriteRepository _teacherWrite,
        UserManager<AppUser> _userManager,
        IMapper _mapper) : ITeacherService
    {
        public async Task<IResult> CreateAsync(CreateTeacherDTO model)
        {
            var appUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            if(!result.Succeeded)
                return new ErrorResult("Müəllim yaradıla bilmədi!");

            var teacher = _mapper.Map<Teacher>(model);

            teacher.AppUserId = appUser.Id;
            teacher.ScientificDegree = model.ScientificDegree;
            teacher.CafedraId = model.CafedraId;

            await _teacherWrite.AddAsync(teacher);
            await _teacherWrite.SaveAsync();

            return new SuccessResult("Müəllim uğurla qeydiyyata alındı");
        }

        public async Task<IDataResult<List<GetTeacherDTO>>> GetAllAsync()
        {
           var teachers = await _teacherRead.GetAll(tracking: false)
                .Include(t => t.AppUser)
                .Include(t => t.Cafedra)
                .ToListAsync();
            
            var dtos = _mapper.Map<List<GetTeacherDTO>>(teachers);

            return new SuccessDataResult<List<GetTeacherDTO>>(dtos);
            
        }

        public async Task<IDataResult<GetTeacherDTO>> GetByIdAsync(Guid id)
        {
            var teacher = await _teacherRead.GetWhere(t => t.Id == id, tracking: false)
                .Include(t => t.AppUser)
                .Include(t => t.Cafedra)
                .FirstOrDefaultAsync();

            if (teacher == null)
                return new ErrorDataResult<GetTeacherDTO>("Müəllim tapılmadı");

            var dto = _mapper.Map<GetTeacherDTO>(teacher);

            return new SuccessDataResult<GetTeacherDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            var teacher = await _teacherRead.GetWhere(t => t.Id == id, tracking: true)
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync();
            if (teacher == null)
                return new ErrorResult("Müəllim tapılmadı");

            await _teacherWrite.HardDeleteAsync(id);
            await _teacherWrite.SaveAsync();

            if(teacher.AppUser != null)
            {
                var identityResult = await _userManager.DeleteAsync(teacher.AppUser);
                if(!identityResult.Succeeded)
                {
                    return new ErrorResult("Müəllim silindi, lakin istifadəçi hesabı silinə bilmədi");
                }
            }
            return new SuccessResult("Müəllim və ona bağlı istifadəçi hesabı bazadan tamamilə silindi");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new ErrorResult("Silmək üçün ID siyahısı boş olamaz.");

            var teachers = await _teacherRead.GetWhere(t => ids.Contains(t.Id), tracking: true)
                .Include(t => t.AppUser)
                .ToListAsync();

            if(!teachers.Any())
                return new ErrorResult("Silmək üçün heç bir müəllim tapılmadı");

            _teacherWrite.HardDeleteRange(teachers);

            await _teacherWrite.SaveAsync();

            foreach(var teacher in teachers)
            {

                if (teacher.AppUser != null)
                {
                    await _userManager.DeleteAsync(teacher.AppUser);
                }
            }

            return new SuccessResult("Seçilmiş müəllimlər və onların istifadəçi hesabları bazadan tamamilə silindi.");
        }



        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            var teacher = await _teacherRead.GetByIdAsync(id, tracking: true);

            if (teacher == null)
                return new ErrorResult("Müəllim tapılmadı.");

            teacher.IsDeleted = true;

            await _teacherWrite.SaveAsync();

            return new SuccessResult("Müəllim passivləşdirildi.");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var teachers = await _teacherRead
                 .GetWhere(s => ids.Contains(s.Id), tracking: true)
                 .ToListAsync();

            if (!teachers.Any())
                return new ErrorResult("Müəllimlər tapılmadı.");

            _teacherWrite.SoftDeleteRange(teachers);

            await _teacherWrite.SaveAsync();

            return new SuccessResult("Müəllimlər passivləşdirildi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _teacherWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _teacherWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }


        public async Task<IResult> UpdateAsync(UpdateTeacherDTO model)
        {
            var teacher = await _teacherRead.GetWhere(t => t.Id == model.Id, tracking : true)
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync();

            if (teacher == null)
                return new ErrorResult("Müəllim tapılmadı");

            _mapper.Map(model, teacher);

            teacher.AppUser.FirstName = model.FirstName;
            teacher.AppUser.LastName = model.LastName;
            teacher.AppUser.Email = model.Email;
            teacher.AppUser.UserName = model.Email;

            var result =  await _userManager.UpdateAsync(teacher.AppUser);

            if (!result.Succeeded)
                return new ErrorResult("Müəllim məlumatları yenilənə bilmədi");

            _teacherWrite.Update(teacher);
            await _teacherWrite.SaveAsync();

            return new SuccessResult("Müəllim məlumatları uğurla yeniləndi");
        }
    }
}
