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

        public Task<IResult> HardDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RestoreAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SoftDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
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
