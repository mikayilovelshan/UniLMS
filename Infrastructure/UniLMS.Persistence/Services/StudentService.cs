using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Students;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.StudentRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class StudentService(IStudentReadRepository _studentRead,
        IStudentWriteRepository _studentWrite,
        UserManager<AppUser> _userManager,
        IMapper _mapper) : IStudentService
    {
        public async Task<IResult> CreateAsync(CreateStudentDTO model)
        {
            var appUser = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName

            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (!result.Succeeded)
                return new ErrorResult("Tələbə yaradıla bilmədi!");

            var student = _mapper.Map<Student>(model);

            student.AppUserId = appUser.Id;
            student.StudentNumber = model.StudentNumber;
            student.GroupId = model.GroupId;
            student.GPA = 0;
            
       
            await _studentWrite.AddAsync(student);
            await _studentWrite.SaveAsync();

            return new SuccessResult("Tələbə uğurla qeydiyyata alındı");
        }

        public async Task<IDataResult<List<GetStudentDTO>>> GetAllAsync()
        {
            var students = await _studentRead.GetAll()
                .Include(s => s.AppUser)
                .Include(s => s.Group)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetStudentDTO>>(students);

            return new SuccessDataResult<List<GetStudentDTO>>(dtos);
        }
        

        public async Task<IDataResult<GetStudentDTO>> GetByIdAsync(Guid id)
        {
           var student = await _studentRead.GetWhere(s => s.Id == id)
                .Include(s => s.AppUser)
                .Include(s => s.Group)
                .FirstOrDefaultAsync();

            if (student == null)
                return new ErrorDataResult<GetStudentDTO>("Tələbə tapılmadı.");

            var studentDTO = _mapper.Map<GetStudentDTO>(student);

            return new SuccessDataResult<GetStudentDTO>(studentDTO);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            var student = await _studentRead.GetByIdAsync(id, tracking: false);

            if (student == null)
                return new ErrorResult("Tələbə tapılmadı.");

            bool isRemoved = await _studentWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Tələbə silinərkən xəta baş verdi.");

            await _studentWrite.SaveAsync();

            var user = await _userManager.FindByIdAsync(student.AppUserId.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return new SuccessResult("Tələbə və istifadəçi hesabı bazadan tamamilə silindi.");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var students = await _studentRead
                .GetWhere(s => ids.Contains(s.Id), tracking: true)
                .ToListAsync();

            if(!students.Any())
                return new ErrorResult("Silmək üçün heç bir tələbə tapılmadı");

            _studentWrite.HardDeleteRange(students);

            await _studentWrite.SaveAsync();

            foreach (var student in students) 
            { 

                if (student.AppUser != null)
                {
                    await _userManager.DeleteAsync(student.AppUser);
                }
            }

            return new SuccessResult("Seçilmiş tələbələr və onların istifadəçi hesabları bazadan tamamilə silindi.");

        }

        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            var student = await _studentRead.GetByIdAsync(id, tracking: true);

            if (student == null)
                return new ErrorResult("Tələbə tapılmadı.");

            student.IsDeleted = true;

            await _studentWrite.SaveAsync();

            return new SuccessResult("Tələbə passivləşdirildi.");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var students = await _studentRead
                .GetWhere(s => ids.Contains(s.Id), tracking: true)
                .ToListAsync();

            if(!students.Any())
                return new ErrorResult("Tələbə tapılmadı.");

            _studentWrite.SoftDeleteRange(students);

            await _studentWrite.SaveAsync();

            return new SuccessResult("Tələbələr passivləşdirildi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _studentWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _studentWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateAsync(UpdateStudentDTO model)
        {
            var student = await _studentRead
                .GetWhere(s => s.Id == model.Id)
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync();

            if (student == null)
                return new ErrorResult("Tələbə tapılmadı.");

            student.AppUser.Email = model.Email;
            student.AppUser.FirstName = model.FirstName;
            student.AppUser.LastName = model.LastName;
            student.AppUser.UserName = model.Email;

            student.GroupId = model.GroupId;
            student.StudentNumber = model.StudentNumber;

            _studentWrite.Update(student);
            await _userManager.UpdateAsync(student.AppUser);
            await _studentWrite.SaveAsync();

            return new SuccessResult("Tələbə məlumatları uğurla yeniləndi.");
        }
    }
}
