using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.AttendanceRepository;
using UniLMS.Application.Repositories.CourseScheduleRepository;
using UniLMS.Application.Utilities.Results;
using UniLMS.Domain.Entities;

namespace UniLMS.Persistence.Services
{
    public class AttendanceService(IAttendanceReadRepository _attendanceRead,
        IAttendanceWriteRepository _attendanceWrite,
        ICourseScheduleReadRepository _courseScheduleRead,
        IMapper _mapper) : IAttendanceService
    {
        public async Task<IResult> CreateRangeAsync(CreateAttendanceRangeDTO model)
        {
            var courseSchedule = await _courseScheduleRead.GetByIdAsync(model.CourseScheduleId, tracking: false);

            if (courseSchedule == null)
                return new ErrorResult("Dərs cədvəli tapılmadı.");

            var attendance = await _attendanceRead.GetWhere(x => x.CourseScheduleId == model.CourseScheduleId && x.Date == model.Date, tracking: false).AnyAsync();

            if (attendance)
                return new ErrorResult("Bu dərs və tarix üçün davamiyyət artıq qeydə alınıb.");

            var studentsId = model.Students.Select(x => x.StudentId).ToList();

            var validStudentsId = await _courseScheduleRead.GetWhere(x => x.Id == model.CourseScheduleId, tracking : false)
                .SelectMany(x => x.CourseOffering.Group.Students)
                .Where(x => !x.IsDeleted)
                .Select(x => x.Id)
                .ToListAsync();

            var invalidStudentsId = studentsId.Except(validStudentsId).ToList();

            if (invalidStudentsId.Any())
                return new ErrorResult("Göndərilən tələbələrdən bir və ya bir neçəsi bu qrupa aid deyil.");

            var attendances = model.Students.Select(s => new Attendance
            {
                Id = Guid.NewGuid(),
                CourseScheduleId = model.CourseScheduleId,
                Date = model.Date,
                StudentId = s.StudentId,
                IsPresent = s.IsPresent,
                Note = s.Note
            }).ToList();

            await _attendanceWrite.AddRangeAsync(attendances);  
            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət uğurla qeydə alındı.");
        }

        public async Task<IDataResult<List<GetAttendanceDTO>>> GetAllAsync()
        {
            var attendances = await _attendanceRead.GetAll(tracking : false)
                .Include(x => x.Student)
                .Include(x => x.CourseSchedule)
                    .ThenInclude(cs => cs.CourseOffering)
                    .ThenInclude(co => co.Group)
                .Include(x => x.CourseSchedule)
                    .ThenInclude(cs => cs.CourseOffering)
                    .ThenInclude(co => co.Teacher)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetAttendanceDTO>>(attendances);

            return new SuccessDataResult<List<GetAttendanceDTO>>(dtos);
        }

        public async Task<IDataResult<GetAttendanceDTO>> GetByIdAsync(Guid id)
        {
            var attendance = await _attendanceRead.GetWhere(x => x.Id == id,tracking: false)
                .Include(x => x.Student)
                .Include(x => x.CourseSchedule)
                    .ThenInclude(cs => cs.CourseOffering)
                    .ThenInclude(co => co.Group)
                .Include(x => x.CourseSchedule)
                    .ThenInclude(cs => cs.CourseOffering)
                    .ThenInclude(co => co.Teacher)
                .FirstOrDefaultAsync();

            if (attendance == null)
                return new ErrorDataResult<GetAttendanceDTO>("Davamiyyət qeydi tapılmadı");

            var dto = _mapper.Map<GetAttendanceDTO>(attendance);

            return new SuccessDataResult<GetAttendanceDTO>(dto);
        }

        public async Task<IResult> HardDeleteAsync(Guid id)
        {
            bool isRemoved = await _attendanceWrite.HardDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Silinəcək davamiyyət qeydi tapılmadı");

            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət qeydi bazadan birdəfəlik silindi.");
        }

        public async Task<IResult> HardDeleteRangeAsync(List<Guid> ids)
        {
            var attendances = await _attendanceRead.GetWhere(x => ids.Contains(x.Id), tracking : true).ToListAsync();

            if (!attendances.Any())
                return new ErrorResult("Silinəcək davamiyyət qeydi tapılmadı");

            if (attendances.Count != ids.Count)
                return new ErrorResult("Göndərilən davamiyyət qeydlərindən bir və ya bir neçəsi sistemdə tapılmadı.");

            _attendanceWrite.HardDeleteRange(attendances);

            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət qeydləri bazadan birdəfəlik silindi.");
        }


        public async Task<IResult> SoftDeleteAsync(Guid id)
        {
            var isRemoved = await _attendanceWrite.SoftDeleteAsync(id);

            if (!isRemoved)
                return new ErrorResult("Silinəcək davamiyyət qeydi tapılmadı");

            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət qeydi bazadan müvəqqəti silindi.");
        }

        public async Task<IResult> SoftDeleteRangeAsync(List<Guid> ids)
        {
            var attendances = await _attendanceRead.GetWhere(x => ids.Contains(x.Id), tracking: true).ToListAsync();

            if (!attendances.Any())
                return new ErrorResult("Silinəcək davamiyyət qeydi tapılmadı");

            if (attendances.Count != ids.Count)
                return new ErrorResult("Göndərilən davamiyyət qeydlərindən bir və ya bir neçəsi sistemdə tapılmadı");

            _attendanceWrite.SoftDeleteRange(attendances);
            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət qeydləri bazadan müvəqqəti silindi.");
        }

        public async Task<IResult> RestoreAsync(Guid id)
        {
            bool isRestored = await _attendanceWrite.RestoreAsync(id);
            if (isRestored == false)
                return new ErrorResult("Silinmiş məlumat tapılmadı və ya aktivdir");
            await _attendanceWrite.SaveAsync();
            return new SuccessResult("Məlumat bərpa edildi");
        }

        public async Task<IResult> UpdateRangeAsync(UpdateAttendanceRangeDTO model)
        {
            var attendanceIds = model.Students.Select(s => s.AttendanceId).ToList();

            var existingAttendances = await _attendanceRead.GetWhere(x => attendanceIds.Contains(x.Id), tracking: true).ToListAsync();

            if (existingAttendances.Any())
                return new ErrorResult("Yenilənəcək davamiyyət qeydi tapılmadı.");

            if (existingAttendances.Count != attendanceIds.Count)
                return new ErrorResult("Göndərilən davamiyyət qeydlərindən bir və ya bir neçəsi tapılmadı.");

            foreach(var attendance in existingAttendances)
            {
                var studentDTO = model.Students.First(x => x.AttendanceId == attendance.Id);

                attendance.IsPresent = studentDTO.IsPresent;
                attendance.Note = studentDTO.Note;
            }

            await _attendanceWrite.SaveAsync();

            return new SuccessResult("Davamiyyət məlumatları uğurla yeniləndi.");
        }
    }
}
